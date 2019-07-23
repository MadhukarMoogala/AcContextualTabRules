using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;


/*
 * <?xml version="1.0" encoding="utf-8"?>
<TabSelectorRules  
  xmlns="clr-namespace:Autodesk.AutoCAD.Ribbon;assembly=AcWindows"
  Ordering="0">
  <TabSelectorRules.References>
     <AssemblyReference
       Namespace="Rules"
       Assembly="Rules"/>
  </TabSelectorRules.References>
  <Rule
    Uid="ADNPluginSampleRuleId"
    DisplayName="Sample: Two circles selected rule"
    Theme="Green"
    Trigger="Selection">
    <![CDATA[
      SampleRule.TheRule.TwoCirclesSelected
    ]]>
  </Rule>
</TabSelectorRules>
 */


namespace Rules
{
    public class SampleRule
    {
        // The flag stating whether the contextual tab is enabled

        bool _enableCtxtTab = false;

        // A static reference to the rule itself

        static SampleRule _rule = null;

        // A public property referenced from the XAML, getting
        // whether the contextual tab should be enabled

        public bool TwoCirclesSelected
        {
            get { return _enableCtxtTab; }
        }

        // A public static property providing access to the rule
        // (also referenced from the XAML)

        public static SampleRule TheRule
        {
            get
            {
                if (_rule == null)
                    _rule = new SampleRule();

                return _rule;
            }
        }

        // Constructor for the rule, where we attach various
        // even handlers

        SampleRule()
        {
            DocumentCollection dm =
              Autodesk.AutoCAD.ApplicationServices.Application.
              DocumentManager;

            dm.DocumentCreated +=
              new DocumentCollectionEventHandler(
                OnDocumentCreated
              );
            dm.DocumentToBeDestroyed +=
              new DocumentCollectionEventHandler(
                OnDocumentToBeDestroyed
              );
            dm.MdiActiveDocument.ImpliedSelectionChanged +=
              new EventHandler(
                OnImpliedSelectionChanged
              );
        }

        // When the pickfirst selection is changed, check
        // the selection to see whether to enable the
        // contextual tab (if exactly two circles are selected)

        void OnImpliedSelectionChanged(object sender, EventArgs e)
        {
            Editor ed =
              Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult res = ed.SelectImplied();
            if (res == null)
                return;

            EnableContextualTab(res.Value);
        }

        // When a document is created, add our handler to it

        void OnDocumentCreated(
          object sender, DocumentCollectionEventArgs e
        )
        {
            e.Document.ImpliedSelectionChanged +=
              new EventHandler(OnImpliedSelectionChanged);
        }

        // When a document is destroyed, remove our handler from it

        void OnDocumentToBeDestroyed(
          object sender, DocumentCollectionEventArgs e
        )
        {
            e.Document.ImpliedSelectionChanged -=
              new EventHandler(OnImpliedSelectionChanged);
        }

        // Check whether we should enable the contextual tab,
        // based on the selection passed in

        void EnableContextualTab(SelectionSet ss)
        {
            // The default assumption is "no"

            _enableCtxtTab = false;

            // We need to have exactly two objects selected

            if (ss == null)
                return;

            if (ss.Count == 2)
            {
                ObjectId[] ids = ss.GetObjectIds();
                if (ids != null)
                {
                    Database db =
                      HostApplicationServices.WorkingDatabase;
                    Transaction tr =
                      db.TransactionManager.StartTransaction();
                    using (tr)
                    {
                        // Check whether both objects are Circles

                        DBObject obj =
                          tr.GetObject(ids[0], OpenMode.ForRead);
                        DBObject obj2 =
                          tr.GetObject(ids[1], OpenMode.ForRead);

                        _enableCtxtTab = (obj is Circle && obj2 is Circle);

                        // Commit, as it's quicker than aborting

                        tr.Commit();
                    }
                }
            }
        }
    }
    
}
