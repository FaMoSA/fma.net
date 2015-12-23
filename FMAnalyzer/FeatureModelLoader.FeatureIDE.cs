using System;
using System.Xml;

namespace Famosa.FMAnalyzer
{
    public class FeatureIDEFeatureModelLoader : FeatureModelLoader
    {
        public FeatureModel load(String filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlElement root = doc.DocumentElement;
            XmlNode node = root.FirstChild;

            // obtain and process tree structure
            XmlNode tree = doc.SelectNodes("/featureModel/struct").Item(0);

            // is not a FeatureIDE XML file ?
            if (tree == null)
                throw new ArgumentException("filename does not refer a FeatureIDE file", "filename");

            // process the tree structure
            FeatureModel model = processTree(tree);

            // obtain and process the constraints
            XmlNodeList constraints = doc.SelectNodes("/featureModel/constraints/rule");
            foreach (XmlNode constraint in constraints)
                processConstraint(model, constraint);

            model.UpdateReferences();

            return model;
        }

        public FeatureModel processTree(XmlNode tree, string prefix = "")
        {
            FeatureModel fm = new FeatureModel();

            // if there is a prefix not ended with a ".", add a "."
            if (prefix != "" && !prefix.EndsWith("."))
                prefix += ".";

            XmlNode root = tree.FirstChild;
            fm.Root = processNode(root, prefix);

            return fm;
        }

        public Feature processNode(XmlNode node, string prefix = "")
        {
            Feature feature = null;
            string name = prefix + node.Attributes["name"].Value;
            string isMandatory = node.Attributes["mandatory"] != null ? node.Attributes["mandatory"].Value : "";
            string isAbstract = node.Attributes["abstract"] != null ? node.Attributes["abstract"].Value : "";

            switch (node.Name)
            {
                case "alt":
                    feature = new AlternativeFeatureGroup(name);
                    break;
                case "and":
                    feature = new AndFeatureGroup(name);
                    break;
                case "or":
                    feature = new OrFeatureGroup(name);
                    break;
                case "feature":
                    feature = new GroupedFeature(name);
                    break;
            }
            feature.IsAbstract = isAbstract == "true" ? true : false;
            feature.IsMandatory = isMandatory == "true" ? true : false;

            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode childNode in children)
            {
                feature.Add(processNode(childNode));
            }

            return feature;
        }

        public FeatureModel processConstraint(FeatureModel model, XmlNode xmlConstraint)
        {
            Constraint constraint = new Constraint("", processConstraintTerm(xmlConstraint.FirstChild));
            model.AddConstraints(constraint);
            return model;
        }

        public ConstraintTerm processConstraintTerm(XmlNode xmlTerm)
        {
            ConstraintTerm term = null;

            switch (xmlTerm.Name)
            {
                case "conj":
                    term = new Conj(
                            processConstraintTerm(xmlTerm.FirstChild),
                            processConstraintTerm(xmlTerm.LastChild)
                        );
                    break;
                case "disj":
                    term = new Disj(
                            processConstraintTerm(xmlTerm.FirstChild),
                            processConstraintTerm(xmlTerm.LastChild)
                        );
                    break;
                case "imp":
                    term = new Implies(
                            processConstraintTerm(xmlTerm.FirstChild),
                            processConstraintTerm(xmlTerm.LastChild)
                        );
                    break;
                case "eq":
                    term = new Equals(
                            processConstraintTerm(xmlTerm.FirstChild),
                            processConstraintTerm(xmlTerm.LastChild)
                        );
                    break;
                case "var":
                    string name = xmlTerm.InnerText;
                    term = new Var(name);
                    break;
                case "not":
                    term = new Not(processConstraintTerm(xmlTerm.FirstChild));
                    break;
            }

            return term;
        }
    }
}
