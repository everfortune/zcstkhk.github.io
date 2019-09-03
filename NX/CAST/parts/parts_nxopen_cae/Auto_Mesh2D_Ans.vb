' Answer file
Option Strict Off
Imports System
Imports NXOpen
Imports NXOpen.UF
Imports NXOpen.CAE
Imports NXOpen.Utilities
Imports NXOpen.UF.UFObj
Imports NXOpenUI ' Added for mesh size prompt <<<<<< 

Module Auto_Mesh2D
    Dim theSession As Session = Session.GetSession()
    Dim theUfSession As UFSession = UFSession.GetUFSession()
    Dim theLW As NXOpen.ListingWindow = theSession.ListingWindow()
    Dim theUI As NXOpen.UI = NXOpen.UI.GetUI

    '  Explicit Activation
    '  This entry point is used to activate the application explicitly
    Sub Main()

        ' Create/initialize object references
        Dim basePart As BasePart = theSession.Parts.BaseWork
        Dim baseFEMPart As CAE.BaseFemPart = CType(basePart, CAE.BaseFemPart)
        Dim baseFEModel As CAE.BaseFEModel = CType(baseFEMPart.BaseFEModel, CAE.BaseFEModel)

        ' Variable definitions/initializations
        Dim myCounter As Integer = 0 'used as counter
        Dim NULLTAG As Tag = NXOpen.Tag.Null
        Dim objectTag As Tag = NXOpen.Tag.Null
        Dim myPropertyTable As CAE.PropertyTable = Nothing
        Dim myPhysicalPropertyTable As CAE.PhysicalPropertyTable = Nothing

        ' Prompt for element size
        Dim myElementSize As Double
        myElementSize = NXInputBox.GetInputNumber("Element Size ", "Element Size ", "10 ")

        theUfSession.Obj.CycleObjsInPart(basePart.Tag, NXOpen.UF.UFConstants.UF_caegeom_type, objectTag)

        Do
            Dim objType As Integer = 0
            Dim objSubType As Integer = 0
            theUfSession.Obj.AskTypeAndSubtype(objectTag, objType, objSubType)
            If objSubType = NXOpen.UF.UFConstants.UF_caegeom_body_subtype Then
                myCounter = myCounter + 1 'used to set the name of the meshcollector

                ' Get the CAEBody object from it's tag - the current returned tag from CycleObjsInPart
                Dim myCAEBody As CAE.CAEBody = NXObjectManager.Get(objectTag)


                ' Get auto element size, set mesh size, thickness
                Dim myAutoElementSize As Double = Nothing
                theUfSession.Sf.GetAutoElementSize(1, myCAEBody.Tag, myAutoElementSize)

                ' Alternate element size options: 
                ' Using a calculation on auto element size
                ' Dim myElementSize As Double = myAutoElementSize / 2.5
                ' 
                ' or a hard coded value
                ' Dim myElementSize As Double = 6.0

                Dim myThickness As Double = myElementSize * 0.1

                ' Create Physical Property Table - set thickness to 10% of auto element size
                Try
                    myPhysicalPropertyTable = baseFEMPart.PhysicalPropertyTables.CreatePhysicalPropertyTable("PSHELL", "NX NASTRAN - Structural", "NX NASTRAN", myThickness.ToString & " THK", 5050 + myCounter)

                    myPropertyTable = myPhysicalPropertyTable.PropertyTable

                    Dim myThicknessExpression As Expression = myPropertyTable.GetScalarPropertyValue("element  thickness")

                    baseFEMPart.Expressions.Edit(myThicknessExpression, myThickness.ToString)
                    myPropertyTable.SetScalarPropertyValue("element thickness", myThicknessExpression)

                Catch
                    myPhysicalPropertyTable = baseFEMPart.PhysicalPropertyTables.FindObject("PhysPropTable[" & myThickness.ToString & " THK]")
                End Try


                ' Create mesh collector
                Dim myMeshManager As CAE.MeshManager = baseFEModel.MeshManager
                Dim MyMeshCollector As CAE.MeshCollector = Nothing
                Dim myCollectorBuilder = myMeshManager.CreateCollectorBuilder(MyMeshCollector, "ThinShell")
                myCollectorBuilder.CollectorName = "Body" & myCounter.ToString & "_Collector"
                If myPhysicalPropertyTable IsNot Nothing Then
                    myCollectorBuilder.PropertyTable.SetNamedPropertyTablePropertyValue("Shell Property", myPhysicalPropertyTable)
                End If
                MyMeshCollector = myCollectorBuilder.Commit()
                myCollectorBuilder.Destroy()

                ' Setup mesh & options
                Dim my2DMesh As CAE.Mesh2d = Nothing
                Dim myMesh2DBuilder As CAE.Mesh2dBuilder = myMeshManager.CreateMesh2dBuilder(my2DMesh)
                ' Mesh options
                myMesh2DBuilder.GeometryUsageType = Mesh2dBuilder.GeometryType.Main
                myMesh2DBuilder.PropertyTable.SetIntegerPropertyValue("meshing method", 0)

                ' Set element size 
                Dim myExpression As Expression
                myExpression = myMesh2DBuilder.PropertyTable.GetScalarPropertyValue("quad mesh overall edge size")
                baseFEMPart.Expressions.Edit(myExpression, (myElementSize).ToString)
                myMesh2DBuilder.PropertyTable.SetScalarPropertyValue("quad mesh overall edge size", myExpression)

                myMesh2DBuilder.PropertyTable.SetBooleanPropertyValue("split poor quads bool", True)
                myMesh2DBuilder.PropertyTable.SetBooleanPropertyValue("mapped mesh option bool", True)
                myMesh2DBuilder.PropertyTable.SetBooleanPropertyValue("mesh format to solver bool", True)
                myMesh2DBuilder.PropertyTable.SetBooleanPropertyValue("mesh transition bool", True)
                myMesh2DBuilder.PropertyTable.SetBooleanPropertyValue("quad mesh edge match toggle", True)
                myMesh2DBuilder.ElementType.ElementDimension = CAE.ElementTypeBuilder.ElementType.Shell
                myMesh2DBuilder.ElementType.ElementTypeName = "CQUAD4"
                myMesh2DBuilder.ElementType.DestinationCollector.AutomaticMode = False
                myMesh2DBuilder.ElementType.DestinationCollector.ElementContainer = MyMeshCollector

                ' Add body to the selectionlist for meshing
                Dim myBool As Boolean = Nothing
                myBool = myMesh2DBuilder.SelectionList.Add(myCAEBody)

                If myBool = True Then
                    ' Mesh faces
                    Dim myMeshes() As CAE.Mesh
                    myMeshes = myMesh2DBuilder.CommitMesh

                    myMesh2DBuilder.Destroy()
                End If

            End If

            theUfSession.Obj.CycleObjsInPart(basePart.Tag, NXOpen.UF.UFConstants.UF_sfem_mesh_type, objectTag)
        Loop While objectTag <> NULLTAG

    End Sub


    Public Function GetUnloadOption(ByVal dummy As String) As Integer


        ' ----Other unload options-------
        ' Unloads the image immediately after execution within NX
        GetUnloadOption = NXOpen.Session.LibraryUnloadOption.Immediately
        ' Unloads the image when the NX session terminates
        ' GetUnloadOption = NXOpen.Session.LibraryUnloadOption.AtTermination

        ' Unloads the image explicitly, via an unload dialog
        ' GetUnloadOption = NXOpen.Session.LibraryUnloadOption.Explicitly
        ' -------------------------------

    End Function

End Module

