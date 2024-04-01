using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParaManager.WinForm.WinForm_Controls;
namespace ParaManager.WinForm
{
    internal class ExtEvent : IExternalEventHandler
    {
        public static Request request { get; set; }
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            using (Transaction trn = new Transaction(doc, "Create"))
            {
                trn.Start();
                switch (request)
                {

                    case Request.createFamilyParameter:
                        rvt.CreateFamilyParameters(FamilyParameterTab.ElementName, FamilyParameterTab.paramNames);
                        break;
                    case Request.associateFamilyParameter:
                        rvt.AssociateFamilyParameters(FamilyParameterTab.ElementName, FamilyParameterTab.paramNames);
                        break;
                    case Request.createGlobalParameter:
                        rvt.CreateGlobalParameters(FamilyParameterTab.ElementName, FamilyParameterTab.paramNames);
                        break;
                    case Request.associateGlobalParameter:
                        rvt.AssociateGlobalParameters(FamilyParameterTab.ElementName, FamilyParameterTab.paramNames);
                        break;
                    default:
                        break;
                }
                
                trn.Commit();
            }
        }

        public string GetName()
        {
            return "MS";
        }
    }
    enum Request
    {
        createFamilyParameter,
        associateFamilyParameter,
        createGlobalParameter,
        associateGlobalParameter
    }
}
