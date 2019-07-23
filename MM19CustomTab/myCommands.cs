// (C) Copyright 2019 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Customization;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(MM19CustomTab.MyCommands))]
[assembly: ExtensionApplication(typeof(MM19CustomTab.MyPlugin))]

namespace MM19CustomTab
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        [CommandMethod("CreateRibbonTabAndPanel")]
        public static void CreateRibbonTabAndPanel_Method()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                CustomizationSection cs = new CustomizationSection((string)Autodesk.AutoCAD.ApplicationServices.Core.Application.GetSystemVariable("MENUNAME"));
                string curWorkspace = (string)Autodesk.AutoCAD.ApplicationServices.Core.Application.GetSystemVariable("WSCURRENT");

                CreateRibbonTabAndPanel(cs, curWorkspace, "MM19Tab", "MM19Panel");
                cs.Save();
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(Environment.NewLine + ex.Message);
            }
        }
        public static void CreateRibbonTabAndPanel(CustomizationSection cs, string toWorkspace, string tabName, string panelName)
        {
            RibbonRoot root = cs.MenuGroup.RibbonRoot;
            RibbonPanelSourceCollection panels = root.RibbonPanelSources;

            //Create The Ribbon Button
            MacroGroup macroGroup = new MacroGroup("MM19macroGroup", cs.MenuGroup);
            MenuMacro macroLine = macroGroup.CreateMenuMacro("TestLine",
                                                            "^C^C_Line",
                                                            "ID_MyLineCmd",
                                                            "My Line help",
                                                             MacroType.Any,
                                                             "RCDATA_16_LINE",
                                                             "RCDATA_32_LINE",
                                                             "My Test Line");

            //Create the ribbon panel source and add it to the ribbon panel source collection
            RibbonPanelSource panelSrc = new RibbonPanelSource(root);
            panelSrc.Text = panelSrc.Name = panelName;
            panelSrc.ElementID = panelSrc.Id = panelName + "_PanelSourceID";
            panels.Add(panelSrc);



            //Create the ribbon tab source and add it to the ribbon tab source collection
            RibbonTabSource tabSrc = new RibbonTabSource(root);
            tabSrc.Name = tabSrc.Text = tabName;
            tabSrc.ElementID = tabSrc.Id = tabName + "_TabSourceID";
            root.RibbonTabSources.Add(tabSrc);

            //Create the ribbon panel source reference and add it to the ribbon panel source reference collection
            RibbonPanelSourceReference ribPanelSourceRef = new RibbonPanelSourceReference(tabSrc)
            {
                PanelId = panelSrc.ElementID
            };

            tabSrc.Items.Add(ribPanelSourceRef);

            // Create a ribbon row 
            RibbonRow ribRow = new RibbonRow(ribPanelSourceRef);
            panelSrc.Items.Add(ribRow);


            //Create a Ribbon Command Button 
            RibbonCommandButton ribCommandButton = new RibbonCommandButton(ribRow)
            {
                Text = "MyTestLineButton",
                MacroID = macroLine.ElementID,

            };
            ribCommandButton.ButtonStyle = RibbonButtonStyle.LargeWithText;
            ribRow.Items.Add(ribCommandButton);
            //Create the workspace ribbon tab source reference
            WSRibbonTabSourceReference tabSrcRef = WSRibbonTabSourceReference.Create(tabSrc);

            //Get the ribbon root of the workspace
            int curWsIndex = cs.Workspaces.IndexOfWorkspaceName(toWorkspace);
            WSRibbonRoot wsRibbonRoot = cs.Workspaces[curWsIndex].WorkspaceRibbonRoot;

            //Set the owner of the ribbon tab source reference and add it to the workspace ribbon tab collection
            tabSrcRef.SetParent(wsRibbonRoot);
            wsRibbonRoot.WorkspaceTabs.Add(tabSrcRef);
        }
    }
    // This line is not mandatory, but improves loading performances




    // This class is instantiated by AutoCAD once and kept alive for the 
    // duration of the session. If you don't do any one time initialization 
    // then you should remove this class.
    public class MyPlugin : IExtensionApplication
    {

        void IExtensionApplication.Initialize()
        {
            // Add one time initialization here
            // One common scenario is to setup a callback function here that 
            // unmanaged code can call. 
            // To do this:
            // 1. Export a function from unmanaged code that takes a function
            //    pointer and stores the passed in value in a global variable.
            // 2. Call this exported function in this function passing delegate.
            // 3. When unmanaged code needs the services of this managed module
            //    you simply call acrxLoadApp() and by the time acrxLoadApp 
            //    returns  global function pointer is initialized to point to
            //    the C# delegate.
            // For more info see: 
            // http://msdn2.microsoft.com/en-US/library/5zwkzwf4(VS.80).aspx
            // http://msdn2.microsoft.com/en-us/library/44ey4b32(VS.80).aspx
            // http://msdn2.microsoft.com/en-US/library/7esfatk4.aspx
            // as well as some of the existing AutoCAD managed apps.

            // Initialize your plug-in application here
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
        }

    }
}
