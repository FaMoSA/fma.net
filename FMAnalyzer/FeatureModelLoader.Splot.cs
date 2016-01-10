using System;
using System.IO;
using System.Xml;
using System.Collections;

namespace Famosa.FMAnalyzer
{
    public class SplotFeatureModelLoader : FeatureModelLoader
    {
        public FeatureModel load(String filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlElement root = doc.DocumentElement;

            // is not a Splot XML file ?
            if (root.Name != "feature_model")
                throw new ArgumentException("filename does not refer a FeatureIDE file", "filename");

            XmlNode node = root.FirstChild;

            // model name
            Console.WriteLine(node.Attributes["name"]);

            // feature tree
            XmlNode tree = doc.SelectNodes("/feature_model/feature_tree").Item(0);
            FeatureModel model = processTree(tree.InnerText);

            // meta data -- optional
            XmlNode metaData = doc.SelectNodes("/feature_model/meta").Item(0);
            model = processMetaData(model, metaData);

            // constraints
            XmlNode constraints = doc.SelectNodes("/feature_model/constraints").Item(0);
            model = processConstraints(model, constraints.InnerText);

            model.UpdateReferences();

            return model;
        }

        private FeatureModel processTree(String treeText)
        {
            FeatureModel fm = new FeatureModel();

            Stack nodeStack = new Stack();

            StringReader reader = new StringReader(treeText);
            String line = skipBlanks(reader);
            while (line != null)
            {
                int level = countTabs(line);
                Feature feature = processLine(line);

                if (level == 0)
                {
                    fm.Root = feature;
                    nodeStack.Push(feature);
                } 
                else
                {
                    int curLevel = nodeStack.Count - 1;
                    if (level > curLevel)
                    {
                        Feature node = (Feature) nodeStack.Peek();
                        node.Add(feature);
                        nodeStack.Push(feature);
                    }
                    else if ( level == curLevel)
                    {
                        nodeStack.Pop();
                        Feature node = (Feature)nodeStack.Peek();
                        node.Add(feature);
                        nodeStack.Push(feature);
                    }
                    else
                    {
                        int countPops = curLevel - level + 1;
                        for (int i = 0; i < countPops; i++)
                        {
                            nodeStack.Pop();
                        }
                        Feature node = (Feature)nodeStack.Peek();
                        node.Add(feature);
                        nodeStack.Push(feature);
                    }
                }

                line = skipBlanks(reader);
            }

            return fm;
        }

        private String skipBlanks(StringReader reader)
        {
            String line = reader.ReadLine();
            while (line != null && line.Length == 0)
                line = reader.ReadLine();
            return line;
        }

        private int countTabs(String line)
        {
            int count = 0;
            int index = line.IndexOf('\t');
            while (index != -1)
            {
                count++;
                index = line.IndexOf('\t', index + 1);
            }
            return count;
        }

        int idCounter = 1;

        private Feature processLine(String line)
        {
            int index = line.IndexOf(":");

            // it is a feature ?
            if (index != -1)
            {
                String nodeType = line.Substring(index + 1, 1).Trim().ToUpper();
                if (nodeType.Length > 0 && !nodeType.Equals("R") && !nodeType.Equals("O")
                    && !nodeType.Equals("M") && !nodeType.Equals("G"))
                {
                    throw new Exception("invalid type for Feature : " + nodeType);
                }

                int findex1 = line.IndexOf("(");
                int findex2 = line.IndexOf(")");
                int findex3 = line.IndexOf("[");

                String featureId;
                String featureName;

                // has ID ?
                if (findex1 != -1)
                {
                    featureId = line.Substring(findex1 + 1, findex2 - findex1 - 1);
                    featureName = line.Substring(index + 2, findex1 - index - 2).Trim();
                    // has ID but not name
                    if (featureName.Length == 0)
                        featureName = featureId;
                }
                // does not have ID but it is a group
                else if (findex3 != -1)
                {
                    featureName = line.Substring(index + 2, findex3).Trim();
                    // does not have ID but it has name
                    if (featureName.Length == 0)
                    {
                        featureName = "_id_" + idCounter++;
                        featureId = featureName.Replace(" ", "_"); // ??
                    }
                    // does not have ID
                    else
                    {
                        featureId = featureName.Replace(" ", "_");
                    }
                } 
                // does not have ID and it is not a group
                else
                {
                    featureName = line.Substring(index + 2).Trim();
                    // does not have ID but it has name
                    if (featureName.Length == 0)
                    {
                        featureName = "_id_" + idCounter++;
                        featureId = featureName.Replace(" ", "_"); // ??
                    }
                    // does not have ID
                    else
                    {
                        featureId = featureName.Replace(" ", "_");
                    }
                }

                Feature feature = null;

                switch (nodeType)
                {
                    // it is a Root feature (concept)
                    case "R":
                        feature = new AndFeatureGroup(featureName);
                        feature.Id = featureId;
                        feature.IsAbstract = false;
                        feature.IsMandatory = true;
                        break;
                    // it is mandatory
                    case "M":
                        feature = new AndFeatureGroup(featureName);
                        feature.Id = featureId;
                        feature.IsAbstract = false;
                        feature.IsMandatory = true;
                        break;
                    // it is optional
                    case "O":
                        feature = new AndFeatureGroup(featureName);
                        feature.Id = featureId;
                        feature.IsAbstract = false;
                        feature.IsMandatory = false;
                        break;
                    // it is a Feature Group
                    case "G":
                        int findex4 = line.IndexOf(",");
                        int findex5 = line.IndexOf("]");

                        String upperBound = line.Substring(findex4 + 1, findex5 - findex4 - 1).Trim();
                        if (upperBound.Equals("*"))
                        {
                            feature = new OrFeatureGroup(featureName);
                            feature.Id = featureId;
                            feature.IsAbstract = false;
                            feature.IsMandatory = true;
                        }
                        else
                        {
                            feature = new AlternativeFeatureGroup(featureName);
                            feature.Id = featureId;
                            feature.IsAbstract = false;
                            feature.IsMandatory = true;
                        }
                        break;
                    // it is a grouped feature
                    default:
                        feature = new GroupedFeature(featureName);
                        feature.Id = featureId;
                        feature.IsAbstract = false;
                        feature.IsMandatory = true;
                        break;
                }
                return feature;

            }
            return null;
        }

        // Meta Data

        public FeatureModel processMetaData(FeatureModel model, XmlNode xmlMetaData)
        {
            if (xmlMetaData != null)
            {
                XmlNodeList children = xmlMetaData.ChildNodes;
                foreach (XmlNode childNode in children)
                {
                    String name = childNode.Attributes["name"].Value;
                    String value = childNode.InnerText;
                    model.MetaData.Add(name, value);
                }
            }

            return model;
        }

        // Constraints

        public FeatureModel processConstraints(FeatureModel model, String constraints)
        {
            StringReader reader = new StringReader(constraints);
            String line = skipBlanks(reader);
            while (line != null)
            {
                int index1 = line.IndexOf(":");
                if (index1 != -1)
                {
                    String constraintName = line.Substring(0, index1).Trim();
                    String constraintText = line.Substring(index1 + 1).Trim();

                    Constraint constraint = new Constraint(constraintName, processConstraintTerm(constraintText));
                    model.AddConstraints(constraint);
                }

                line = skipBlanks(reader);
            }

            return model;
        }

        public ConstraintTerm processConstraintTerm(String constraintText)
        {
            string[] parts = constraintText.Split(' ');

            ConstraintTerm term = null;

            int index1 = constraintText.IndexOf(" or ");

            if (index1 != -1)
            {
                term = new Disj(
                        processConstraintTerm(constraintText.Substring(0, index1)),
                        processConstraintTerm(constraintText.Substring(index1 + 4))
                        );
            }
            else
            {
                string text = constraintText.Trim();
                if ( text.StartsWith("~"))
                {
                    term = new Not(processConstraintTerm(text.Substring(1)));
                }
                else
                {
                    term = new Var(text);
                }
            }

            return term;
        }
    }

}
