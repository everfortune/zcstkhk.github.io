' NX 8.5.0.23
' Journal created by lacombe on Tue Oct 16 11:36:20 2012 Eastern Daylight Time
'
Option Strict Off
Imports System
Imports NXOpen
' Added BlockStyler and UF import  
Imports NXOpen.UF
Imports NXOpen.BlockStyler

Module NXJournal
    Dim theSession As Session = Session.GetSession()
    Dim theLW As ListingWindow = theSession.ListingWindow()

    Sub Main()
        ' Added workDir variable
        Dim workDir As String = "D:\NX\Bottle\"
        ' Added this dimension statement
        Dim mesh As CAE.Mesh = Nothing

        ' New function for calling meshing routine
        MeshBottle(mesh)
    End Sub

    Sub MeshBottle(ByRef mesh As CAE.Mesh)

        Dim workFemPart As CAE.FemPart = CType(theSession.Parts.BaseWork, CAE.FemPart)
        Dim displayFemPart As CAE.FemPart = CType(theSession.Parts.BaseDisplay, CAE.FemPart)
        ' Added code for mesh dialog >>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim objectsToMesh(0) As DisplayableObject

        Dim elmSize As String = "3"
        Dim elmSizeUnit As Unit = Nothing
        ' Added (Nothing) below to allow changing the search location for dlx files
        Dim thebottleMeshDb As bottleMeshDb = Nothing
        ' theLW.Open()
        Try
            thebottleMeshDb = New bottleMeshDb()

            ' The following method shows the dialog immediately
            thebottleMeshDb.Show()

            If thebottleMeshDb.faces.Length < 1 Then Return
            ReDim objectsToMesh(thebottleMeshDb.faces.Length - 1)
            Dim Idx As Integer
            Idx = 0

            For Each Item As NXOpen.CAE.CaeFace In thebottleMeshDb.faces
                objectsToMesh(Idx) = CType(Item, DisplayableObject)
                Idx = Idx + 1
            Next

            elmSizeUnit = thebottleMeshDb.unit

            ' theLW.WriteLine("Output from Mesh Bottle UI L54")
            ' theLW.WriteLine("The number of faces selected is  " + objectsToMesh.Length.ToString)
            If elmSizeUnit Is Nothing Then
                ' theLW.WriteLine("The Element Size is   " + elmSize + "[ unitless]")
            Else
                ' theLW.WriteLine("The Element Size is   " + elmSize + "[" + elmSizeUnit.Abbreviation + "]")

            End If

            ' A (Return) in the program will halt execution at that point
            'Return
        Catch ex As Exception

            '---- Enter your exception handling code here ----

        Finally
            ' Added the following lines for error checking
            If thebottleMeshDb IsNot Nothing Then
                thebottleMeshDb.Dispose()
            End If

        End Try
        ' End of added code <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        Dim markId1 As Session.UndoMarkId
        markId1 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Start")
        'Added Code >>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim caePart1 As CAE.CaePart = CType(workFemPart, CAE.CaePart)

        Dim markId3 As Session.UndoMarkId
        markId3 = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Start")
        ' End of added code <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        Dim fEModel1 As CAE.FEModel = CType(workFemPart.FindObject("FEModel"), CAE.FEModel)

        Dim meshManager1 As CAE.MeshManager = CType(fEModel1.Find("MeshManager"), CAE.MeshManager)

        Dim nullCAE_Mesh2d As CAE.Mesh2d = Nothing

        Dim mesh2dBuilder1 As CAE.Mesh2dBuilder
        mesh2dBuilder1 = meshManager1.CreateMesh2dBuilder(nullCAE_Mesh2d)
       mesh2dBuilder1.GeometryUsageType = CAE.Mesh2dBuilder.GeometryType.Main
        mesh2dBuilder1.ElementType.DestinationCollector.AutomaticMode = False

        Dim meshCollector1 As CAE.MeshCollector = CType(meshManager1.FindObject("MeshCollector[TestPlates_f_ThinShell(1)]"), CAE.MeshCollector)

mesh2dBuilder1.ElementType.DestinationCollector.ElementContainer = meshCollector1

        theSession.SetUndoMarkName(markId1, "2D Mesh Dialog")

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("meshing method", 1)
        Dim expression1 As Expression
        expression1 = mesh2dBuilder1.PropertyTable.GetScalarPropertyValue("quad mesh overall edge size")
        Dim unit1 As Unit = CType(workFemPart.UnitCollection.FindObject("MilliMeter"), Unit)
        Dim cAEBody1 As CAE.CAEBody = CType(workFemPart.FindObject("CAE_Body(1)"), CAE.CAEBody)

        Dim cAEFace31 As CAE.CAEFace = CType(cAEBody1.FindObject("CAE_Face(10)"), CAE.CAEFace)
        Dim added2 As Boolean
        added2 = mesh2dBuilder1.SelectionList.Add(objectsToMesh)

        Dim markId4 As Session.UndoMarkId
        markId4 = theSession.SetUndoMark(Session.MarkVisibility.Invisible, "2D Mesh")

        mesh2dBuilder1.ElementType.ElementDimension = CAE.ElementTypeBuilder.ElementType.Shell

        mesh2dBuilder1.ElementType.ElementTypeName = "CQUAD4"

        Dim destinationCollectorBuilder1 As CAE.DestinationCollectorBuilder
        destinationCollectorBuilder1 = mesh2dBuilder1.ElementType.DestinationCollector

        Dim nullCAE_MeshCollector As CAE.MeshCollector = Nothing

        destinationCollectorBuilder1.ElementContainer = nullCAE_MeshCollector

        destinationCollectorBuilder1.AutomaticMode = True

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("meshing method", 1)

mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("quad mesh overall edge size", "3", unit1)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("mapped mesh option bool", True)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("fillet num elements", 3)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("num elements on cylinder circumference", 3)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("element size on cylinder height", "1", unit1)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("quad only option", 0)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("midnodes", 2)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("split poor quads bool", False)

        Dim nullUnit As Unit = Nothing

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("max quad warp", "5", nullUnit)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("max jacobian", "5", nullUnit)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("mesh format to solver bool", True)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("mesh transition bool", False)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("mesh size variation", "50", nullUnit)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("quad mesh edge match toggle", True)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("quad mesh edge match tolerance", "0.02", unit1)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("small feature tolerance", "10", nullUnit)

        mesh2dBuilder1.PropertyTable.SetBooleanPropertyValue("merge edge toggle bool", False)

        Dim unit2 As Unit = CType(workFemPart.UnitCollection.FindObject("Degrees"), Unit)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("edge angle", "15", unit2)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("quad mesh smoothness tolerance", "0.01", unit1)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("quad mesh surface match tolerance", "0.001", unit1)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("quad mesh transitional rows", 3)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("min face angle", "20", unit2)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("mesh time stamp", 0)

        mesh2dBuilder1.PropertyTable.SetScalarWithDataPropertyValue("quad mesh node coincidence tolerance", "0.0001", unit1)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("mesh edit allowed", 0)
        Dim expression14 As Expression
        expression14 = mesh2dBuilder1.PropertyTable.GetScalarPropertyValue("quad mesh overall edge size")

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("transition edge seeding", 0)
        workFemPart.Expressions.EditWithUnits(expression14, unit1, "3")
        workFempart.Expressions.EditWithUnits(expression14, elmSizeUnit, elmSize)

        mesh2dBuilder1.PropertyTable.SetIntegerPropertyValue("cylinder curved end num elements", 6)

        mesh2dBuilder1.AutoResetOption = False

        Dim id1 As Session.UndoMarkId
        id1 = theSession.NewestVisibleUndoMark

        Dim nErrs1 As Integer
        nErrs1 = theSession.UpdateManager.DoUpdate(id1)

        Dim meshes1() As CAE.Mesh
        meshes1 = mesh2dBuilder1.CommitMesh()

        theSession.SetUndoMarkName(id1, "2D Mesh")

        ' These test lines to mark a location in the List Window output
        ' Return line is to stop program execution if desired.
        ' theLW.WriteLine("Got to the point just before the Destroy line at L214, exiting")
        ' theLW.WriteLine("---------------------------------------------------")
        'Return

        mesh2dBuilder1.Destroy()

        ' ----------------------------------------------
        '   Menu: Tools->Journal->Stop Recording
        ' ----------------------------------------------

    End Sub
    Public Class bottleMeshDb
        'class members
        Private Shared theSession As Session
        ' Added code >>>>>>>>>>>>>>
        Public faces As NXOpen.CAE.CaeFace()
        Public exp As Expression
        Public unit As Unit
        Public formula As String

        Private Shared theUI As UI
        Private theDlxFileName As String
        ' Added the (=Nothing) to the end of the next line
        Private theDialog As NXOpen.BlockStyler.BlockDialog = Nothing
        Private group0 As NXOpen.BlockStyler.Group ' Block type: Group
        Private selection0 As NXOpen.BlockStyler.SelectObject ' Block type: Selection
        Private expression0 As NXOpen.BlockStyler.ExpressionBlock ' Block type: Expression
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

                ' theLW.WriteLine("Into BlockStyler Try Loop-284") ' Match this line# to your code
                theSession = Session.GetSession()
                Dim journal As String = theSession.ExecutingJournal
                Dim testFile As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(journal)
                Dim folderPath As String = testFile.DirectoryName
                ' dialog is expected to be next to the vb file.
                theDlxFileName = My.Computer.FileSystem.CombinePath(folderPath, "bottleMeshDb.dlx")
                testFile = My.Computer.FileSystem.GetFileInfo(theDlxFileName)
                If Not testFile.Exists Then
                    theDlxFileName = "bottleMeshDb.dlx"
                End If

                theUI = UI.GetUI()
                theDialog = theUI.CreateDialog(theDlxFileName)
                theDialog.AddApplyHandler(AddressOf apply_cb)
                theDialog.AddOkHandler(AddressOf ok_cb)
                theDialog.AddUpdateHandler(AddressOf update_cb)
                theDialog.AddInitializeHandler(AddressOf initialize_cb)
                theDialog.AddDialogShownHandler(AddressOf dialogShown_cb)

                ' Added Code >>>>>>>>>>>>>>>>>>>>>>>>
                ReDim faces(0)
                exp = Nothing
                unit = Nothing
                formula = ""
                ' End of added code <<<<<<<<<<<<<<<<<<<<<<<

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
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
            Dim thebottleMeshDb As bottleMeshDb = Nothing
            Try

                ' theLW.WriteLine("Into xxxxMain Try Loop-L349")
                thebottleMeshDb = New bottleMeshDb()
                ' The following method shows the dialog immediately
                thebottleMeshDb.Show()
                For Each Item As NXOpen.TaggedObject In thebottleMeshDb.faces
                    Dim face As NXOpen.CAE.CaeFace = CType(Item, NXOpen.CAE.CaeFace)
                Next

            Catch ex As Exception

                '---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
            Finally
                If thebottleMeshDb IsNot Nothing Then
                    thebottleMeshDb.Dispose()
                    thebottleMeshDb = Nothing
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
            ' theLW.WriteLine("GetUloadOption Function-L381")
            'Return CType(Session.LibraryUnloadOption.Explicitly, Integer)
            Return CType(Session.LibraryUnloadOption.Immediately, Integer)
            ' Return CType(Session.LibraryUnloadOption.AtTermination, Integer)
        End Function
        '------------------------------------------------------------------------------
        ' Following method cleanup any housekeeping chores that may be needed.
        ' This method is automatically called by NX.
        '------------------------------------------------------------------------------
        Public Shared Sub UnloadLibrary(ByVal arg As String)
            ' theLW.WriteLine("UnloadLibrary Function-L391")
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
                expression0 = CType(theDialog.TopBlock.FindBlock("expression0"), NXOpen.BlockStyler.UIBlock)
                ' Added Code >>>>>>>>>>>
                Dim propertyList As BlockStyler.PropertyList
                propertyList = selection0.GetProperties()

                Dim masktriples1(0) As Selection.MaskTriple
                masktriples1(0).Type = NXOpen.UF.UFConstants.UF_caegeom_type
                masktriples1(0).Subtype = NXOpen.UF.UFConstants.UF_caegeom_face_subtype
                masktriples1(0).SolidBodySubtype = -1
                propertyList.SetSelectionFilter("SelectionFilter", Selection.SelectionAction.ClearAndEnableSpecific, masktriples1)
                propertyList.SetString("LabelString", "Select Faces to Mesh")
                propertyList = group0.GetProperties()
                propertyList.SetString("Label", "Bottle Mesh Settings")
                propertyList = expression0.GetProperties()
                propertyList.SetString("Label", "Element Size")
                ' End of added code <<<<<<<<<<<<<<<<<<  

                propertyList.SetDouble("MaximumValue", 999999999.9)
                propertyList.SetDouble("MinimumValue", 0.0001)

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

                ' Added code >>>>>>>>>>>>>>>>>>>>  
                selection0 = theDialog.TopBlock.FindBlock("selection0")
                expression0 = theDialog.TopBlock.FindBlock("expression0")

                'Retrieve the selection0 property list  
                Dim propertyList As BlockStyler.PropertyList
                propertyList = selection0.GetProperties()

                Dim objects As NXOpen.TaggedObject()

                objects = propertyList.GetTaggedObjectVector("SelectedObjects")

                Dim Lng As Integer = 0

                ReDim faces(objects.Length - 1)

                For Each Item As NXOpen.TaggedObject In objects

                    Try

                        Dim body As NXOpen.CAE.CaeFace = CType(Item, NXOpen.CAE.CaeFace)
                        If body Is Nothing Then
                            ' Error of some sort 
                        Else
                            faces(Lng) = body
                            Lng = Lng + 1

                        End If

                    Catch ex As ApplicationException

                    End Try

                    '---- Enter your callback code here -----
                Next
                ' Uncomment the next line to open the List Window for debugging
                ' Dim theLW As ListingWindow = theSession.ListingWindow()
                ' theLW.Open()

                propertyList = expression0.GetProperties()
                Dim expTag As TaggedObject

                Dim unitTag As TaggedObject

                expTag = propertyList.GetTaggedObject("ExpressionObject")

                If expTag IsNot Nothing Then
                    exp = CType(expTag, Expression)
                    Dim rhs As String = exp.RightHandSide()

                    Dim expName As String = exp.Name

                    ' theLW.WriteLine(" We have a exp named = " + expName + " RHS = " + rhs)

                End If
                unitTag = propertyList.GetTaggedObject("Units")
                If unitTag IsNot Nothing Then
                    unit = CType(unitTag, Unit)
                    Dim unitName As String = unit.Name
                    Dim unitabbr As String = unit.Abbreviation
                    ' Added the following debugging statements
                    ' theLW.WriteLine(" < - - - First line output to the List Window L551 - - - >")
                    ' theLW.WriteLine(" Marking the start helps during multiple debugging runs.")
                    ' Added blank separator line after output start marker lines.
                    ' theLW.WriteLine(" ")
                    ' theLW.WriteLine(" We have a unit named = " + unitName + " Abbreviation = " + unitabbr)

                End If

                formula = propertyList.GetString("Formula")
                If formula IsNot Nothing Then

                    ' theLW.WriteLine(" Formula = " + formula)

                    ' theLW.WriteLine(" ")
                    ' theLW.WriteLine(" The List Window can be used to dump variables for debugging, or to show progress during journal execution L585")
                    ' theLW.WriteLine(" ")

                End If
                ' End added code <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

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
            ' theLW.WriteLine("update_cb Function-L584")
            Try

                If block Is selection0 Then
                    '---- Enter your code here -----

                ElseIf block Is expression0 Then
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