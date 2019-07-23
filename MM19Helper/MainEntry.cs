// (C) Copyright 2019 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows.Data;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(MM19Helper.Functions))]
[assembly: ExtensionApplication(null)]

namespace MM19Helper
{


    /*
     * 
<?xml version="1.0" encoding="utf-8"?>
<TabSelectorRules xmlns="clr-namespace:Autodesk.AutoCAD.Ribbon;assembly=AcWindows" Ordering="0">
    <TabSelectorRules.References>
        <AssemblyReference Namespace="MM19Helper" Assembly="MM19Helper" />
    </TabSelectorRules.References>
    <Rule Uid="MM19SelectionRule" DisplayName="MM19 Selecion Rule" Theme="Green" Trigger="Selection">
        <![CDATA[MM19Helper.Functions.ShowMyTab(Selection)]]>
    </Rule>
</TabSelectorRules>
     */

    public class Functions
    {

        public static bool ShowMyTab(object selObj)
        {

            Selection sel = (Selection)selObj;
            if (sel.Count < 1 || !sel.ContainsOnly(new string[] { "Line" }))
                return false;

            if (Application.DocumentManager.MdiActiveDocument.Name.EndsWith("Drawing1.dwg"))
                return true;
            return false;

        }
    }
}
