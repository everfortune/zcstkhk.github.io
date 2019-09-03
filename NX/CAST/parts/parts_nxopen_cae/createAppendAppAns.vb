' NX 8.5.0.23
' Journal created by lacombe on Thu Oct 11 10:52:33 2012 Eastern Daylight Time
'
Option Strict Off
Imports System
Imports NXOpen
' Added for BlockStyler UI pieces
Imports NXOpen.Blockstyler

Module NXJournal
    Dim theSession As Session = Session.GetSession()
    Dim theLW As NXOpen.ListingWindow = theSession.ListingWindow()
    Dim theUI As NXOpen.UI = NXOpen.UI.GetUI()

    Sub Main()

        ' Base for any filename path - location of DLX files and TestPlates directory
        Dim workDir As String = "D:\UG\"
        ' ---------------------------------------
        ' Add other function calls below
        CreateFem(workDir)
        AppendFem(workDir)
    End Sub
    Sub CreateFem(ByRef workDir As String)

        Dim theSession As Session = Session.GetSession()
        Dim workPart As Part = theSession.Parts.Work

        Dim displayPart As Part = theSession.Parts.Display

        Dim body1 As Body
        Dim theSolidBodySelection As faceSelect = Nothing
        Dim fembodies() As Body
        Try
            theSolidBodySelection = New faceSelect()

            ' The following method shows the dialog immediately
            theSolidBodySelection.Show()

            fembodies = theSolidBodySelection.bodies.Clone()
            If fembodies.Length < 1 Then Return

            body1 = fembodies(0)

        Catch ex As Exception

        Finally
            ' Added the following lines for error checking
            If theSolidBodySelection IsNot Nothing Then
                theSolidBodySelection.Dispose()
            End If

        End Try

        Dim markId1 As Session.UndoMarkId
        markId1 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Start")

        Dim fileNew1 As FileNew
        fileNew1 = theSession.Parts.FileNew()

        theSession.SetUndoMarkName(markId1, "New Part File Dialog")

        ' ----------------------------------------------
        '   Dialog Begin New Part File
        ' ----------------------------------------------
        Dim markId2 As Session.UndoMarkId
        markId2 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Enter Advanced Simulation")
        Dim markId3 As Session.UndoMarkId ' Changed Id to 3
        markId3 = theSession.SetUndoMark(Session.MarkVisibility.Invisible, "New Part File")

        fileNew1.TemplateFileName = "BottleMaterial.fem"

        fileNew1.Application = FileNewApplication.CaeFem

        fileNew1.Units = Part.Units.Millimeters

        fileNew1.TemplateType = FileNewTemplateType.Item

        Dim femPartName As String = workDir + "bottle_f.fem"
        fileNew1.NewFileName = femPartName

        fileNew1.MasterFileName = ""

        fileNew1.UseBlankTemplate = False

        fileNew1.MakeDisplayedPart = True

        theSession.DeleteUndoMark(markId3, Nothing)

        theSession.SetUndoMarkName(markId1, "New Part File")

        Dim nXObject1 As NXObject
        nXObject1 = fileNew1.Commit()

        workPart = Nothing
        Dim workFemPart As CAE.FemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)

        displayPart = Nothing
        Dim displayFemPart As CAE.FemPart = CType(theSession.Parts.BaseDisplay, CAE.FemPart)

        fileNew1.Destroy()

        Dim markId31 As Session.UndoMarkId
        markId31 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Start")

        theSession.SetUndoMarkName(markId31, "New FEM Dialog")
        ' Added lines >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim part1 As Part = CType(theSession.Parts.FindObject("bottle"), Part)

        Dim partLoadStatus1 As PartLoadStatus
        Dim status1 As PartCollection.SdpsStatus
        status1 = theSession.Parts.SetDisplay(part1, False, True, partLoadStatus1)

        workFemPart = Nothing
        workPart = theSession.Parts.Work
        displayFemPart = Nothing
        displayPart = theSession.Parts.Display
        ' End of added lines <<<<<<<<<<<<<<<<<<<<<<<<<<
        ' ----------------------------------------------
        '   Dialog Begin New FEM
        ' ----------------------------------------------
        Dim markId4 As Session.UndoMarkId
        markId4 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "New FEM")

        theSession.SetUndoMarkName(markId4, "New FEM Dialog")

        Dim markId5 As Session.UndoMarkId
        markId4 = theSession.SetUndoMark(Session.MarkVisibility.Invisible, "New FEM")

        theSession.DeleteUndoMark(markId5, Nothing)

        Dim markId6 As Session.UndoMarkId
        markId6 = theSession.SetUndoMark(Session.MarkVisibility.Invisible, "New FEM")

        Dim femPart1 As CAE.FemPart = CType(nXObject1, CAE.FemPart)

        Dim partLoadStatus2 As PartLoadStatus
        Dim status2 As PartCollection.SdpsStatus
        status2 = theSession.Parts.SetDisplay(femPart1, False, True, partLoadStatus2)
        workPart = Nothing
        workFemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)
        displayPart = Nothing
        displayFemPart = CType(theSession.Parts.BaseDisplay, CAE.FemPart)
        Dim femPart2 As CAE.FemPart = CType(workFemPart, CAE.FemPart)

        theSession.Parts.SetWork(femPart2)
        workFemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)
        ' End of added lines <<<<<<<<<<<<<<<<<<<<
        Dim femPart3 As CAE.FemPart = CType(workFemPart, CAE.FemPart)

        Dim femSynchronizeOptions1 As CAE.FemSynchronizeOptions
        femSynchronizeOptions1 = femPart3.NewFemSynchronizeOptions()

        femSynchronizeOptions1.SynchronizePointsFlag = False

        femSynchronizeOptions1.SynchronizeCoordinateSystemFlag = False

        femSynchronizeOptions1.SynchronizeLinesFlag = False

        femSynchronizeOptions1.SynchronizeArcsFlag = False

        femSynchronizeOptions1.SynchronizeSplinesFlag = False

        femSynchronizeOptions1.SynchronizeConicsFlag = False

        femSynchronizeOptions1.SynchronizeSketchCurvesFlag = False

        Dim femPart4 As CAE.FemPart = CType(workFemPart, CAE.FemPart)

        Dim bodies1(0) As Body

        bodies1(0) = body1
        Dim description1(-1) As String
        ' Changed filename to use workDir variable
        Dim iPartName As String = workDir + "bottle_f_i.prt"
        femPart4.FinalizeCreation(part1, iPartName, CAE.FemPart.UseBodiesOption.SelectedBodies, bodies1, femSynchronizeOptions1, "NX NASTRAN", "Structural", description1)

        workFemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)

        femSynchronizeOptions1.Dispose()
        theSession.DeleteUndoMark(markId4, Nothing)

        ' ----------------------------------------------
        '   Menu: Tools->Journal->Stop Recording
        ' ----------------------------------------------

        ' ----------------------------------------------
        '   Menu: File->Save All
        ' ----------------------------------------------
        Dim anyPartsModified1 As Boolean
        Dim partSaveStatus1 As PartSaveStatus
        theSession.Parts.SaveAll(anyPartsModified1, partSaveStatus1)
        partSaveStatus1.Dispose()

    End Sub
    Sub AppendFem(ByRef workDir As String)

        Dim theSession As Session = Session.GetSession()
        Dim workFemPart As CAE.FemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)

        Dim displayFemPart As CAE.FemPart = CType(theSession.Parts.BaseDisplay, CAE.FemPart)

        Dim markId1 As Session.UndoMarkId
        markId1 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Start")

        theSession.SetUndoMarkName(markId1, "Append Fem Dialog")

        Dim basePart1 As BasePart
        Dim partLoadStatus1 As PartLoadStatus
        Dim femTestPlateName As String = workDir + "TestPlates\TestPlates_f.fem"
        basePart1 = theSession.Parts.OpenBase(femTestPlateName, partLoadStatus1)

        partLoadStatus1.Dispose()
        Dim markId2 As Session.UndoMarkId
        markId2 = theSession.SetUndoMark(Session.MarkVisibility.Invisible, "Append Fem")

        Dim femPart1 As CAE.FemPart = CType(workFemPart, CAE.FemPart)

        Dim baseFEModel1 As CAE.BaseFEModel
        baseFEModel1 = femPart1.BaseFEModel

        Dim femPart2 As CAE.FemPart = CType(basePart1, CAE.FemPart)

        Dim baseFEModel2 As CAE.BaseFEModel
        baseFEModel2 = femPart2.BaseFEModel

        Dim fEModel1 As CAE.FEModel = CType(baseFEModel1, CAE.FEModel)

        Dim fEModel2 As CAE.FEModel = CType(baseFEModel2, CAE.FEModel)

        Dim idSpec1 As CAE.FEModel.IdSpecificationObject
        idSpec1.FemObjectPrependName = "TestPlates_f"
        idSpec1.NodeStartId = 1
        idSpec1.NodeIdOffset = True
        idSpec1.ElementStartId = 1
        idSpec1.ElementIdOffset = True
        idSpec1.PhysicalPropertyTableStartId = 2
        idSpec1.PhysicalPropertyTableIdOffset = False
        fEModel1.AppendFemodel(fEModel2, idSpec1)

        theSession.DeleteUndoMark(markId2, Nothing)

        theSession.SetUndoMarkName(markId1, "Append Fem")

        ' ----------------------------------------------
        '   Menu: Fit
        workFemPart.ModelingViews.WorkView.Fit()
        ' ----------------------------------------------
        ' Menu: File->Save (added)
        Dim femPart5 As CAE.FemPart = CType(workFemPart, CAE.FemPart)
        Dim partSaveStatus1 As PartSaveStatus
        partSaveStatus1 = femPart5.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.False)
        partSaveStatus1.Dispose()

        ' ----------------------------------------------
        '   Menu: Tools->Journal->Stop Recording
        ' ----------------------------------------------

    End Sub
    Public Class faceSelect
        'class members
        Private Shared theSession As Session
        ' Added the following line to allow access to a listing window
        Private Shared theLW As NXOpen.ListingWindow
        Private Shared theUI As UI
        Public Shared thefaceSelect As faceSelect
        ' Added the following for body selection
        Public bodies As NXOpen.Body()
        Private theDlxFileName As String
        ' Added the (= Nothing) to the end of the next line
        Private theDialog As NXOpen.BlockStyler.BlockDialog = Nothing
        Private group0 As NXOpen.BlockStyler.Group ' Block type: Group
        Private selection0 As NXOpen.BlockStyler.SelectObject ' Block type: Selection
        ' Added next line for an expression
        Private expression0 As NXOpen.BlockStyler.UIBlock ' Block type: Expression
        '------------------------------------------------------------------------------
        'Bit Option for Property: SnapPointTypesEnabled
        '------------------------------------------------------------------------------
        Public Shared ReadOnly SnapPointTypesEnabled_UserDefined As Integer = 1
        Public Shared ReadOnly SnapPointTypesEnabled_Inferred As Integer = 2
        Public Shared ReadOnly SnapPointTypesEnabled_ScreenPosition As Integer = 4
        Public Shared ReadOnly SnapPointTypesEnabled_EndPoint As Integer = 8
        Public Shared ReadOnly SnapPointTypesEnabled_MidPoint As Integer = 16
        Public Shared ReadOnly SnapPointTypesEnabled_ControlPoint As Integer = 32
        Public Shared ReadOnly SnapPointTypesEnabled_Intersection As Integer = 64
        Public Shared ReadOnly SnapPointTypesEnabled_ArcCenter As Integer = 128
        Public Shared ReadOnly SnapPointTypesEnabled_QuadrantPoint As Integer = 256
        Public Shared ReadOnly SnapPointTypesEnabled_ExistingPoint As Integer = 512
        Public Shared ReadOnly SnapPointTypesEnabled_PointonCurve As Integer = 1024
        Public Shared ReadOnly SnapPointTypesEnabled_PointonSurface As Integer = 2048
        Public Shared ReadOnly SnapPointTypesEnabled_PointConstructor As Integer = 4096
        Public Shared ReadOnly SnapPointTypesEnabled_TwocurveIntersection As Integer = 8192
        Public Shared ReadOnly SnapPointTypesEnabled_TangentPoint As Integer = 16384
        Public Shared ReadOnly SnapPointTypesEnabled_Poles As Integer = 32768
        Public Shared ReadOnly SnapPointTypesEnabled_BoundedGridPoint As Integer = 65536
        '------------------------------------------------------------------------------
        'Bit Option for Property: SnapPointTypesOnByDefault
        '------------------------------------------------------------------------------
        Public Shared ReadOnly SnapPointTypesOnByDefault_EndPoint As Integer = 8
        Public Shared ReadOnly SnapPointTypesOnByDefault_MidPoint As Integer = 16
        Public Shared ReadOnly SnapPointTypesOnByDefault_ControlPoint As Integer = 32
        Public Shared ReadOnly SnapPointTypesOnByDefault_Intersection As Integer = 64
        Public Shared ReadOnly SnapPointTypesOnByDefault_ArcCenter As Integer = 128
        Public Shared ReadOnly SnapPointTypesOnByDefault_QuadrantPoint As Integer = 256
        Public Shared ReadOnly SnapPointTypesOnByDefault_ExistingPoint As Integer = 512
        Public Shared ReadOnly SnapPointTypesOnByDefault_PointonCurve As Integer = 1024
        Public Shared ReadOnly SnapPointTypesOnByDefault_PointonSurface As Integer = 2048
        Public Shared ReadOnly SnapPointTypesOnByDefault_PointConstructor As Integer = 4096
        Public Shared ReadOnly SnapPointTypesOnByDefault_BoundedGridPoint As Integer = 65536

#Region "Block Styler Dialog Designer generator code"
        '------------------------------------------------------------------------------
        'Constructor for NX Styler class
        '------------------------------------------------------------------------------
        Public Sub New()
            Try

                theSession = Session.GetSession()
                Dim journal As String = theSession.ExecutingJournal
                Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(journal)
                Dim folderPath As String = testFile.DirectoryName

                ' dialog is expect to to be next to vb file. 
                theDlxFileName = My.Computer.FileSystem.CombinePath(folderPath, "faceSelect.dlx")

                testFile = My.Computer.FileSystem.GetFileInfo(theDlxFileName)
                If Not testFile.Exists Then
                    theDlxFileName = "faceSelect.dlx"
                End If

                theUI = UI.GetUI()
                theDialog = theUI.CreateDialog(theDlxFileName)
                theDialog.AddApplyHandler(AddressOf apply_cb)
                theDialog.AddOkHandler(AddressOf ok_cb)
                theDialog.AddUpdateHandler(AddressOf update_cb)
                theDialog.AddInitializeHandler(AddressOf initialize_cb)
                theDialog.AddDialogShownHandler(AddressOf dialogShown_cb)
                ReDim bodies(0)
            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Bottle Crush Example", NXMessageBox.DialogType.Error, ex.ToString)
                Throw ex
            End Try
        End Sub
#End Region

        '------------------------------- DIALOG LAUNCHING ---------------------------------
        '
        '    Before invoking this application one needs to open any part/empty part in NX
        '    because of the behavior of the blocks.
        '
        '    Make sure the dlx file is in one of the following locations:
        '        1.) From where NX session is launched
        '        2.) $UGII_USER_DIR/application
        '        3.) For released applications, using UGII_CUSTOM_DIRECTORY_FILE is highly
        '            recommended. This variable is set to a full directory path to a file 
        '            containing a list of root directories for all custom applications.
        '            e.g., UGII_CUSTOM_DIRECTORY_FILE=$UGII_ROOT_DIR\menus\custom_dirs.dat
        '
        '    You can create the dialog using one of the following way:
        '
        '    1. Journal Replay
        '
        '        1) Replay this file through Tool->Journal->Play Menu.
        '
        '    2. USER EXIT
        '
        '        1) Create the Shared Library -- Refer "Block UI Styler programmer's guide"
        '        2) Invoke the Shared Library through File->Execute->NX Open menu.
        '
        '------------------------------------------------------------------------------
        Public Shared Sub xxxxMain()
            Dim thefaceSelect As faceSelect = Nothing
            Try

                thefaceSelect = New faceSelect()
                ' The following method shows the dialog immediately
                thefaceSelect.Show()
                For Each Item As NXOpen.TaggedObject In thefaceSelect.bodies

                    Dim faces As NXOpen.Body = CType(Item, NXOpen.Body)

                Next

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            Finally
                If thefaceSelect IsNot Nothing Then
                    thefaceSelect.Dispose()
                    thefaceSelect = Nothing
                End If
            End Try
        End Sub
        '------------------------------------------------------------------------------
        ' This method specifies how a shared image is unloaded from memory
        ' within NX. This method gives you the capability to unload an
        ' internal NX Open application or user  exit from NX. Specify any
        ' one of the three constants as a return value to determine the type
        ' of unload to perform:
        '
        '
        '    Immediately : unload the library as soon as the automation program has completed
        '    Explicitly  : unload the library from the "Unload Shared Image" dialog
        '    AtTermination : unload the library when the NX session terminates
        '
        '
        ' NOTE:  A program which associates NX Open applications with the menubar
        ' MUST NOT use this option since it will UNLOAD your NX Open application image
        ' from the menubar.
        '------------------------------------------------------------------------------
        Public Shared Function GetUnloadOption(ByVal arg As String) As Integer
            'Return CType(Session.LibraryUnloadOption.Explicitly, Integer)
            Return CType(Session.LibraryUnloadOption.Immediately, Integer)
            ' Return CType(Session.LibraryUnloadOption.AtTermination, Integer)
        End Function
        '------------------------------------------------------------------------------
        ' Following method cleanup any housekeeping chores that may be needed.
        ' This method is automatically called by NX.
        '------------------------------------------------------------------------------
        Public Shared Sub UnloadLibrary(ByVal arg As String)
            Try


            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
        End Sub

        '------------------------------------------------------------------------------
        'This method shows the dialog on the screen
        '------------------------------------------------------------------------------
        Public Sub Show()
            Try

                theDialog.Show()

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
        End Sub

        '------------------------------------------------------------------------------
        'Method Name: Dispose
        '------------------------------------------------------------------------------
        Public Sub Dispose()
            If theDialog IsNot Nothing Then
                theDialog.Dispose()
                theDialog = Nothing
            End If
        End Sub

        '------------------------------------------------------------------------------
        '---------------------Block UI Styler Callback Functions--------------------------
        '------------------------------------------------------------------------------

        '------------------------------------------------------------------------------
        'Callback Name: initialize_cb
        '------------------------------------------------------------------------------
        Public Sub initialize_cb()
            Try

                group0 = CType(theDialog.TopBlock.FindBlock("group0"), NXOpen.BlockStyler.UIBlock)
                selection0 = CType(theDialog.TopBlock.FindBlock("selection0"), NXOpen.BlockStyler.UIBlock)

                ' Some additional Code
                expression0 = theDialog.TopBlock.FindBlock("expression0")

                Dim propertyList As BlockStyler.PropertyList
                propertyList = selection0.GetProperties()

                Dim masktriples1(0) As Selection.MaskTriple
                masktriples1(0).Type = NXOpen.UF.UFConstants.UF_solid_type
                masktriples1(0).Subtype = NXOpen.UF.UFConstants.UF_solid_body_subtype
                masktriples1(0).SolidBodySubtype = NXOpen.UF.UFConstants.UF_UI_SEL_FEATURE_BODY
                propertyList.SetSelectionFilter("SelectionFilter", Selection.SelectionAction.ClearAndEnableSpecific, masktriples1)

                propertyList.SetString("LabelString", "Bottle Solid Body")

                propertyList = group0.GetProperties()
                propertyList.SetString("Label", "Body Selection")
                ' End of additions

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
        End Sub

        '------------------------------------------------------------------------------
        'Callback Name: dialogShown_cb
        'This callback is executed just before the dialog launch. Thus any value set 
        'here will take precedence and dialog will be launched showing that value. 
        '------------------------------------------------------------------------------
        Public Sub dialogShown_cb()
            Try

                '---- Enter your callback code here -----

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
        End Sub

        '------------------------------------------------------------------------------
        'Callback Name: apply_cb
        '------------------------------------------------------------------------------
        Public Function apply_cb() As Integer
            Dim errorCode As Integer = 0
            Try

                '---- Enter your callback code here -----
                ' All of this was added
                selection0 = theDialog.TopBlock.FindBlock("selection0")

                ' Retrieve the selection0 property list
                Dim propertyList As BlockStyler.PropertyList
                propertyList = selection0.GetProperties()

                Dim objects As NXOpen.TaggedObject()

                objects = propertyList.GetTaggedObjectVector("SelectedObjects")

                Dim Lng As Integer = 0

                ReDim bodies(objects.Length - 1)

                For Each Item As NXOpen.TaggedObject In objects

                    Try

                        Dim body As NXOpen.Body = CType(Item, NXOpen.Body)
                        If body Is Nothing Then
                            ' Error of some sort
                        Else
                            bodies(Lng) = body
                            Lng = Lng + 1
                        End If
                    Catch ex As ApplicationException

                    End Try
                Next

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                errorCode = 1
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
            apply_cb = errorCode
        End Function

        '------------------------------------------------------------------------------
        'Callback Name: update_cb
        '------------------------------------------------------------------------------
        Public Function update_cb(ByVal block As NXOpen.BlockStyler.UIBlock) As Integer
            Try

                If block Is selection0 Then
                    '---- Enter your code here -----

                End If

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
            update_cb = 0
        End Function

        '------------------------------------------------------------------------------
        'Callback Name: ok_cb
        '------------------------------------------------------------------------------
        Public Function ok_cb() As Integer
            Dim errorCode As Integer = 0
            Try

                '---- Enter your callback code here -----
                errorCode = apply_cb()

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                errorCode = 1
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
            ok_cb = errorCode
        End Function

        '------------------------------------------------------------------------------
        'Function Name: GetBlockProperties
        'Returns the propertylist of the specified BlockID
        '------------------------------------------------------------------------------
        Public Function GetBlockProperties(ByVal blockID As String) As PropertyList
            GetBlockProperties = Nothing
            Try

                GetBlockProperties = theDialog.GetBlockProperties(blockID)

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            End Try
        End Function

    End Class
End Module