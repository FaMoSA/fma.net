using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Famosa.FMAnalyzer
{
    public class NamedElement
    {
        public string Name { get; set; }

        public NamedElement(string name)
        {
            if (!name.All(Char.IsLetter))
                this.Name = removeInvalidCharsFromName(name);
            else
                this.Name = name;
        }

        private static String removeInvalidCharsFromName(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!Char.IsLetter(c) && !c.Equals('_') && !Char.IsNumber(c))
                    continue;
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }


    public class FeatureModel
    {
        public Feature Root { get; set; }

        public List<Feature> Features { get; set; }
        public Dictionary<String, Feature> FeatureMap { get; set; }

        public List<FeatureRelationship> Relationships { get; set; }
        public List<Constraint> Constraints { get; set; }

        public Dictionary<String, String> MetaData { get; set; }

        public FeatureModel()
        {
            this.Features = new List<Feature>();
            this.FeatureMap = new Dictionary<string, Feature>();
            this.Relationships = new List<FeatureRelationship>();
            this.Constraints = new List<Constraint>();
            this.MetaData = new Dictionary<string, string>();
            this.Root = null;
        }

        public FeatureModel(Feature root) : this()
        {
            this.Root = root;
        }

        public void AddRelationships(params FeatureRelationship[] relationships)
        {
            foreach (FeatureRelationship relationship in relationships)
                Relationships.Add(relationship);
        }

        public void AddConstraints(params Constraint[] constraints)
        {
            foreach (Constraint constraint in constraints)
                Constraints.Add(constraint);
        }

        public void UpdateReferences()
        {
            UpdateFeatureMap(Root);
            FixMandatoryFeatures(Root);     // Errors in FeatureIDE files
            foreach (Constraint constraint in this.Constraints)
                UpdateConstraint(constraint.Term);
        }

        private void UpdateFeatureMap(Feature feature)
        {
            if (feature.Id == null || feature.Id.Length == 0)
                feature.Id = feature.Name;

            if (this.FeatureMap.ContainsKey(feature.Id))
                throw new InvalidOperationException("feature " + feature.Id + " is included two times");

            this.Features.Add(feature);
            this.FeatureMap.Add(feature.Id, feature);
            foreach (Feature childFeature in feature.Children)
                UpdateFeatureMap(childFeature);
        }

        private void UpdateConstraint(ConstraintTerm term)
        {
            if (term is Var)
            {
                Var var = (Var)term;
                Feature feature;
                this.FeatureMap.TryGetValue(var.Name, out feature);
                var.Feature = feature;
            }
            else if (term is Not)
            {
                Not notTerm = (Not)term;
                UpdateConstraint(notTerm.Term);
            }
            else if (term is BinaryConstraintTerm)
            {
                BinaryConstraintTerm binTerm = (BinaryConstraintTerm)term;
                UpdateConstraint(binTerm.Left);
                UpdateConstraint(binTerm.Right);
            }
        }

        private void FixMandatoryFeatures(Feature feature)
        {
            if (feature is AlternativeFeatureGroup || feature is OrFeatureGroup)
                foreach (Feature childFeature in feature.Children)
                    childFeature.IsMandatory = false;

            foreach (Feature childFeature in feature.Children)
                FixMandatoryFeatures(childFeature);
        }
    }

    abstract public class FeatureModelElement : NamedElement
    {
        public Boolean IsAbstract { get; set; }
        public Boolean IsMandatory { get; set; }

        protected FeatureModelElement() : base("") { }

        public FeatureModelElement(string name) : base(name)
        {
            this.IsAbstract = false;
            this.IsMandatory = false;
        }
    }

    abstract public class Feature : FeatureModelElement
    {
        public enum FeatureValue
        {
            Unknown = -1,
            Deselected = 0,
            Selected = 1
        }

        public string Id { get; set; }
        public string Description { get; set; }

        public List<Feature> Children { get; set; }

        public FeatureValue value { get; set; }

        public Feature(String name) : base(name)
        {
            Children = new List<Feature>();
        }

        public Feature(string name, params Feature[] children) : this(name)
        {
            foreach (Feature feature in children)
                this.Add(feature);
        }

        abstract public void Add(Feature feature);

        protected void add(Feature feature)
        {
            this.Children.Add(feature);
        }

        // IsAncestorOf(...)
        // IsDescendantOf(...)

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class RootFeature : Feature
    {
        public RootFeature(string name) : base(name) { }

        public RootFeature(string name, params Feature[] children) : base(name, children) { }

        override public void Add(Feature feature)
        {
            add(feature);
            /*
            if (!(feature is GroupedFeature))
                add(feature);
            else
                throw new ArgumentException("Trying to add a GroupedFeature to Root");
            */
        }
    }

    public class FeatureGroup : Feature
    {
        public FeatureGroup(string name, int min, int max) : base(name)
        {
            this.min = min;
            this.max = max;
        }

        public FeatureGroup(string name, int min, int max, params Feature[] children) : base(name, children)
        {
            this.min = min;
            this.max = max;
        }

        public int min { get; set; }
        public int max { get; set; }

        override public void Add(Feature feature)
        {
            add(feature);
            /*
            if (feature is GroupedFeature)
                add(feature);
            else
                throw new ArgumentException("Trying to add a non GroupedFeature to a Group");
            */
        }

    }

    public class AndFeatureGroup : FeatureGroup
    {
        public AndFeatureGroup(string name) : base(name, 0, -1) { }
        public AndFeatureGroup(string name, params Feature[] children) : base(name, 0, -1, children) { }
    }

    public class OrFeatureGroup : FeatureGroup
    {
        public OrFeatureGroup(string name) : base(name, 1, -1) { }
        public OrFeatureGroup(string name, params Feature[] children) : base(name, 1, -1, children) { }
    }

    public class AlternativeFeatureGroup : FeatureGroup
    {
        public AlternativeFeatureGroup(string name) : base(name, 1, 1) { }
        public AlternativeFeatureGroup(string name, params Feature[] children) : base(name, 1, 1, children) { }
    }

    public class GroupedFeature : Feature
    {
        public GroupedFeature(string name) : base(name) { }
        public GroupedFeature(string name, params Feature[] children) : base(name, children) { }

        override public void Add(Feature feature)
        {
            add(feature);
            /*
            if (!(feature is GroupedFeature))
                add(feature);
            else
                throw new ArgumentException("Trying to add a GroupedFeature to a GroupedFeature");
            */
        }
    }

    public class SolitaireFeature : Feature
    {
        public SolitaireFeature(string name, bool isOptional) : base(name)
        {
            this.IsOptional = isOptional;
        }
        public SolitaireFeature(string name, bool isOptional, params Feature[] children) : base(name, children)
        {
            this.IsOptional = isOptional;
        }

        bool IsOptional { get; set; }

        override public void Add(Feature feature)
        {
            add(feature);
            /*
            if (!(feature is GroupedFeature))
                add(feature);
            else
                throw new ArgumentException("Trying to add a GroupedFeature to a SolitaireFeature");
            */
        }
    }

    public class FeatureRelationship : FeatureModelElement
    {
        public string SourceName { get; set; }
        public string TargetName { get; set; }

        public Feature Source { get; set; }
        public Feature Target { get; set; }

        public FeatureRelationship(Feature source, Feature target)
        {
            this.Source = source;
            this.Target = target;
        }

        public FeatureRelationship(string source, string target)
        {
            this.SourceName = source;
            this.TargetName = target;
        }
    }

    public class ExcludesRelationship : FeatureRelationship
    {
        public ExcludesRelationship(Feature source, Feature target) : base(source, target) { }
        public ExcludesRelationship(string source, string target) : base(source, target) { }
    }

    public class RequiresRelationship : FeatureRelationship
    {
        public RequiresRelationship(Feature source, Feature target) : base(source, target) { }
        public RequiresRelationship(string source, string target) : base(source, target) { }
    }

    public class Constraint : NamedElement
    {
        public ConstraintTerm Term { get; set; }

        public Constraint() : base("") { }
        public Constraint(ConstraintTerm term) : base("")
        {
            this.Term = term;
        }
        public Constraint(string name, ConstraintTerm term) : base(name)
        {
            this.Term = term;
        }
    }

    public abstract class ConstraintTerm { }

    public class Var : ConstraintTerm
    {
        public string Name { get; set; }
        public Feature Feature { get; set; }

        public Var(string name)
        {
            this.Name = name;
        }
    }

    public class Not : ConstraintTerm
    {
        public ConstraintTerm Term { get; set; }

        public Not(ConstraintTerm term)
        {
            this.Term = term;
        }
    }

    public class BinaryConstraintTerm : ConstraintTerm
    {
        public ConstraintTerm Left { get; set; }
        public ConstraintTerm Right { get; set; }

        public BinaryConstraintTerm(ConstraintTerm left, ConstraintTerm right)
        {
            this.Left = left;
            this.Right = right;
        }
    }

    public class Conj : BinaryConstraintTerm
    {
        public Conj(ConstraintTerm left, ConstraintTerm right) : base(left, right) { }
    }

    public class Disj : BinaryConstraintTerm
    {
        public Disj(ConstraintTerm left, ConstraintTerm right) : base(left, right) { }
    }

    public class Implies : BinaryConstraintTerm
    {
        public Implies(ConstraintTerm left, ConstraintTerm right) : base(left, right) { }
    }

    public class Equals : BinaryConstraintTerm
    {
        public Equals(ConstraintTerm left, ConstraintTerm right) : base(left, right) { }
    }

    public class Configuration
    {
        public List<Feature> features { get; set; }
        public Dictionary<Feature, Feature.FeatureValue> featureValues { get; set; }

        public Configuration()
        {
            features = new List<Feature>();
            featureValues = new Dictionary<Feature, Feature.FeatureValue>();
        }

        public void AddFeaturesByName(FeatureModel model, String[] features)
        {
            foreach (string feature in features)
            {
                AddFeature(model.FeatureMap[feature]);
            }
        }

        public void AddFeature(Feature f)
        {
            if (!featureValues.ContainsKey(f))
                features.Add(f);
            featureValues.Add(f, f.value);
        }

        public bool Contains(Feature f)
        {
            return featureValues.ContainsKey(f);
        }

        override public string ToString()
        {
            string text = "";
            foreach (Feature f in features)
            {
                if (text != "")
                    text += ", ";
                text += f.Name;
            }
            return text;
        }
    }
}