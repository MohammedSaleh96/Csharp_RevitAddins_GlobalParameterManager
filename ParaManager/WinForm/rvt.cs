using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParaManager.WinForm
{
    internal class rvt
    {
        #region Hilfe Methods
        public static IList<Element> GetElementsInDocument(Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .WhereElementIsNotElementType();

            //FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));
            IList<Element> elements = col.ToElements();
            return elements;
        }

        public static List<String> getFamilyInstancesName(Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));
            IList<Element> elements = col.ToElements().ToList();
            List<string> elemsName = new List<string>();

            foreach (var element in elements)
            {
                string name = element.Name;
                if (!elemsName.Contains(name))
                {
                    elemsName.Add(name);
                }
            }
            return elemsName;
        }
        public static Element getElementByName(string parametername)
        {
            Element selectedElement = rvt.GetElementsInDocument(ExtCmd.doc)
                    .FirstOrDefault(item => item.Name == parametername);
            return selectedElement;
        }

        public static List<Element> getElementsByName(string elementName)
        {

            List<Element> selectedElements = rvt.GetElementsInDocument(ExtCmd.doc)
                .Where(item => item.Name == elementName)
                .ToList();
            return selectedElements;
        }
        #endregion

        #region Haupt_Methods
        public static void CreateFamilyParameters(string elementName, List<String> parametersName)
        {
            Element element = getElementByName(elementName);
            IEnumerable<FamilyParameter> famParameters = ExtCmd.doc.FamilyManager.Parameters.Cast<FamilyParameter>();


            foreach (string paramName in parametersName)
            {
                bool parameterExists = famParameters.Any(parameter => parameter.Definition.Name == paramName);
                if (!parameterExists)
                {
                    foreach (Parameter parameter in element.Parameters)
                    {
                        // Check if the current parameter's name matches the paramName
                        if (parameter.Definition.Name == paramName)
                        {
                            // Get the groupId and typeId of the parameter
                            string parameterName = parameter.Definition.Name;
                            ForgeTypeId parameterGroupId = parameter.Definition.GetGroupTypeId();
                            ForgeTypeId parameterTypeId = parameter.Definition.GetDataType();
                            //bool Instance = true;
                            // Create a Family Parameter with the same name, groupId, and typeId
                            FamilyParameter famparameter = ExtCmd.doc.FamilyManager.AddParameter(paramName, parameterGroupId, parameterTypeId, true);
                            //ExtCmd.doc.FamilyManager.Set(famparameter, parameter.AsDouble());

                            // Note: The last argument 'false' specifies that the parameter is not shared.
                        }
                    }
                }
                else
                {
                    // Handle the case where the parameter already exists, e.g., show a message or log it.
                    Console.WriteLine($"Parameter '{paramName}' already exists in the Family document.");
                }
            }

        }

        public static void AssociateFamilyParameters(string elementName, List<String> parametersName)
        {
            Element element = getElementByName(elementName);
            List<Element> elements = getElementsByName(elementName);

            foreach (FamilyParameter famParameter in ExtCmd.doc.FamilyManager.Parameters)
            {
                foreach (Element ele in elements)
                {
                    foreach (Parameter parameter in element.Parameters)
                    {
                        foreach (string parameterName in parametersName)
                        {
                            if (parameter.Definition.Name == famParameter.Definition.Name && parameter.Definition.Name == parameterName)
                            {
                                ExtCmd.doc.FamilyManager.AssociateElementParameterToFamilyParameter(parameter, famParameter);
                            }
                        }
                    }
                }
            }

        }

        public static void CreateGlobalParameters(string elementName, List<String> parametersName)
        {
            //// Remove the duplicated parameters
            List<string> newParametername = new List<string>();

            foreach (string num in parametersName)
            {
                // Check if the element is not already in the unique list
                if (!newParametername.Contains(num))
                {
                    newParametername.Add(num);
                }
            }
            
            
            Element element = getElementByName(elementName);
            IEnumerable<GlobalParameter> globalParameters = GlobalParametersManager.GetAllGlobalParameters(ExtCmd.doc)
                .Select(id => ExtCmd.doc.GetElement(id))
                .OfType<GlobalParameter>();

            foreach (string paramName in newParametername)
            {
                
                // Check if a global parameter with the same name already exists
                bool parameterExists = globalParameters.Any(parameter => parameter.Name == paramName);

                if (!parameterExists)
                {
                    foreach (Parameter parameter in element.Parameters)
                    {
                        // Check if the current parameter's name matches the paramName
                        if (parameter.Definition.Name == paramName)
                        {
                            // Get the groupId and typeId of the parameter
                            string parameterName = parameter.Definition.Name;
                            ForgeTypeId parameterTypeId = parameter.Definition.GetDataType();

                            // Create a Global Parameter with the same name, groupId, and typeId
                            GlobalParameter.Create(ExtCmd.doc, parameterName, parameterTypeId);
                            break; // Exit the loop since you've created the global parameter
                        }
                    }
                }
                else
                {
                    // Handle the case where the parameter already exists
                    Console.WriteLine($"Global Parameter '{paramName}' already exists.");
                }
            }



        }
        public static void AssociateGlobalParameters(string elementName, List<string> parametersName)
        {
            ///
            /// Of of the Update here, is to make sure that there is no duplicated parameters, Either from the user selection or from the parameters inside the Element
            /// 
            //// Remove the duplicated parameters
            List<string> newParametername = new List<string>();

            foreach (string num in parametersName)
            {
                // Check if the element is not already in the unique list
                if (!newParametername.Contains(num))
                {
                    newParametername.Add(num);
                }
            }
            // Check if a global parameter with the same name already exists in the Family document
            IEnumerable<GlobalParameter> globalParameters = GlobalParametersManager.GetAllGlobalParameters(ExtCmd.doc)
            .Select(id => ExtCmd.doc.GetElement(id))
            .OfType<GlobalParameter>();

            List<Element> elements = getElementsByName(elementName);
            // lists to check the strings
            List<string> check = new List<string>();
            List<Parameter> newelementparameter = new List<Parameter>();
            foreach (GlobalParameter gloParameter in globalParameters)
            {
                foreach (Element element in elements)
                {
                    // Make sure there is no duplication in the parameters in the elements
                    Element elementt = getElementByName(elementName);
                    foreach (Parameter parameters in elementt.Parameters)
                    {
                        // Check if the element is not already in the unique list
                        if (!check.Contains(parameters.Definition.Name))
                        {

                            check.Add(parameters.Definition.Name);
                            newelementparameter.Add(parameters);
                        }
                    }
                    foreach (Parameter parameter in newelementparameter)
                    {
                        foreach (string parameterName in newParametername)
                        {
                            if (parameter.Definition.Name == gloParameter.Name && parameter.Definition.Name == parameterName)
                            {
                                parameter.AssociateWithGlobalParameter(gloParameter.Id);
                            }
                        }
                    }
                }
            }


        }
        #endregion
    }
}
