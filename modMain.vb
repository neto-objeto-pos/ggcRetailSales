Imports System.Windows.Forms
Imports System.Reflection
Imports ggcAppDriver

Module modMain
    Private Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInteger, ByVal dwExtraInfo As UInteger)

    'This method can handle all events using EventHandler
    Public Sub grpEventHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As EventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(RadioButton) Then
                        Dim loObj = DirectCast(loTxt, RadioButton)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Label) Then
                        Dim loObj = DirectCast(loTxt, Label)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpEventHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using CancelEventHandler
    Public Sub grpCancelHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As System.ComponentModel.CancelEventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpCancelHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using KeyEventHandler
    Public Sub grpKeyHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As KeyEventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpKeyHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using EventHandler
    Public Function FindRadioButton(ByVal foParent As Control, ByVal fsName As String) As Control
        Dim loTxt As Control
        Static loRet As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = GetType(RadioButton) Then
                'Handle events for this controls only
                If LCase(loTxt.Name) = LCase(fsName) Then
                    loRet = loTxt
                End If
            Else
                If loTxt.HasChildren Then
                    Call FindRadioButton(loTxt, fsName)
                End If
            End If
        Next 'loTxt In loControl.Controls

        Return loRet
    End Function

    'This method can handle all events using EventHandler
    Public Function FindLabel(ByVal foParent As Control, ByVal fsName As String) As Control
        Dim loTxt As Control
        Static loRet As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = GetType(Label) Then
                'Handle events for this controls only
                If LCase(loTxt.Name) = LCase(fsName) Then
                    loRet = loTxt
                End If
            Else
                If loTxt.HasChildren Then
                    Call FindLabel(loTxt, fsName)
                End If
            End If
        Next 'loTxt In loControl.Controls

        Return loRet
    End Function

    'This method can handle all events using EventHandler
    Public Function FindTextBox(ByVal foParent As Control, ByVal fsName As String) As Control
        Dim loTxt As Control
        Static loRet As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = GetType(TextBox) Then
                'Handle events for this controls only
                If LCase(loTxt.Name) = LCase(fsName) Then
                    loRet = loTxt
                End If
            Else
                If loTxt.HasChildren Then
                    Call FindTextBox(loTxt, fsName)
                End If
            End If
        Next 'loTxt In loControl.Controls

        Return loRet
    End Function

    Public Sub SetNextFocus()
        keybd_event(&H9, 0, 0, 0)
        keybd_event(&H9, 0, &H2, 0)
    End Sub

    Public Sub SetPreviousFocus()
        keybd_event(&H10, 0, 0, 0)
        keybd_event(&H9, 0, 0, 0)
        keybd_event(&H10, 0, &H2, 0)
    End Sub

    'Public Function computeTotal(foMaster As DataTable, _
    '                          foDetail As DataTable, _
    '                          foDtaDisc As DataTable, _
    '                          foDiscount As Discount) As Boolean
    '    Dim lnRow As Integer
    '    Dim lnTotal As Double = 0.0
    '    Dim lnSlPrc As Double = 0.0
    '    Dim lnDiscntbl As Double = 0.0
    '    Dim lnZeroRatd As Double = 0.0
    '    Dim lnDiscount As Double = 0

    '    For lnRow = 0 To foDetail.Rows.Count - 1
    '        'Is the detail not complement
    '        'Note: complements are not included in our computation
    '        'If foDetail(lnRow).Item("nComplmnt") = 0 Then
    '        'Is the item has a promo?
    '        If foDetail(lnRow).Item("nAddDiscx") > 0 Or foDetail(lnRow).Item("nAddDiscx") > 0 Then
    '            'Compute sales price for this item
    '            lnSlPrc = (foDetail(lnRow).Item("nUnitPrce") * _
    '                        (100 - foDetail(lnRow).Item("nDiscount")) / 100 -
    '                        foDetail(lnRow).Item("nAddDiscx")) * _
    '                    IIf(foDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                    (foDetail(lnRow).Item("nQuantity") - foDetail(lnRow).Item("nComplmnt"))
    '            'Compute for the total price
    '            lnTotal += lnSlPrc
    '        Else
    '            'Is there a card discount
    '            If foDtaDisc Is Nothing Then
    '                'No card discount was detected 
    '                'Compute sales price for this item
    '                lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * _
    '                          IIf(foDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                          (foDetail(lnRow).Item("nQuantity") - foDetail(lnRow).Item("nComplmnt"))

    '                'Compute for the total price
    '                lnTotal += lnSlPrc
    '            Else
    '                'A discount card is detected
    '                'Is there a particular category for this discount card
    '                If foDtaDisc(0).Item("sCategrID") = "" Then
    '                    'No particular category
    '                    'Compute sales price for this item
    '                    lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * _
    '                              IIf(foDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                              (foDetail(lnRow).Item("nQuantity") - foDetail(lnRow).Item("nComplmnt"))

    '                    'Compute for the total price
    '                    lnTotal += lnSlPrc

    '                    'Compute for the total discountable amount for the card 
    '                    lnDiscntbl += lnSlPrc

    '                    lnDiscount += (foDetail(lnRow).Item("nAddDiscx") * IIf(foDetail(lnRow).Item("cReversex") = "+", 1, -1))

    '                Else
    '                    'Only a particular category has discount for this card
    '                    Dim loDta() As DataRow
    '                    loDta = foDtaDisc.Select("sCategrID = " & strParm(foDtaDisc(0).Item("sCategrID")))

    '                    'Is the item part of discountable category for this card
    '                    If loDta.Count > 0 Then
    '                        'This is a discountable category for this card
    '                        'Compute sales price for this item
    '                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * (foDetail(lnRow).Item("nQuantity") - foDetail(lnRow).Item("nComplmnt"))

    '                        Dim lnDiscx As Decimal
    '                        lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + loDta(0).Item("nDiscAmtx")

    '                        If foDetail(lnRow).Item("cReversex") = "-" Then
    '                            lnSlPrc = lnSlPrc * -1
    '                            lnDiscx = lnDiscx * -1
    '                        End If

    '                        'Compute for discount
    '                        lnDiscount = lnDiscount + lnDiscx

    '                        'Compute for the total price
    '                        lnTotal += lnSlPrc
    '                    Else
    '                        'This is not discountable category for this card
    '                        'Compute sales price for this item
    '                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * _
    '                                  IIf(foDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                                  (foDetail(lnRow).Item("nQuantity") - foDetail(lnRow).Item("nComplmnt"))

    '                        'Compute for the total price
    '                        lnTotal += lnSlPrc
    '                    End If
    '                End If
    '            End If
    '        End If
    '        'kalyptus - 2016.11.28 11:33am
    '        'TODO: Adding of tag whether an ITEM is Vatable or NOT...
    '        'If foDetail(lnRow).Item("cNoneVATx") = "1" Then
    '        '    lnZeroRatd += lnSlPrc
    '        'End If
    '        'End If
    '    Next

    '    foMaster(0).Item("nTranTotl") = lnTotal
    '    foMaster(0).Item("nDiscntbl") = lnDiscntbl
    '    foMaster(0).Item("nZeroRatd") = lnZeroRatd

    '    Dim lnVatPerc As Double = 1.12

    '    'Compute for a Card Discount without category
    '    If Not foDtaDisc Is Nothing Then
    '        If foDtaDisc(0).Item("sCategrID") = "" Then
    '            Dim lnDisc As Decimal

    '            foDiscount.Master("nDiscRate") = foDtaDisc(0).Item("nDiscRate")
    '            foDiscount.Master("nAddDiscx") = lnDiscount

    '            If foDiscount.Master("cNoneVatx") = "1" Then
    '                'Assume that NON-Vat discount is for SENIOR CITIZEN/PWD card holder only
    '                Dim lnAmtx As Decimal = lnTotal / foDiscount.Master("nNoClient") * foDiscount.Master("nWithDisc")
    '                Dim lnNonVat As Decimal

    '                lnNonVat = Math.Round(lnAmtx / lnVatPerc, 2)
    '                foMaster(0).Item("nNonVATxx") = lnNonVat
    '                foMaster(0).Item("nVatDiscx") = lnAmtx - lnNonVat

    '                lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnNonVat
    '                lnDisc += foDtaDisc(0).Item("nDiscAmtx")

    '                foMaster(0).Item("nPWDDiscx") = lnDisc
    '            Else
    '                lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnDiscntbl
    '                lnDisc += foDtaDisc(0).Item("nDiscAmtx")
    '                foMaster(0).Item("nDiscount") = lnDisc
    '            End If
    '        Else
    '            foDiscount.Master("nDiscRate") = 0
    '            foDiscount.Master("nAddDiscx") = lnDiscount
    '            foMaster(0).Item("nDiscount") = lnDiscount
    '        End If
    '    Else
    '        If Not IsNothing(foDiscount) Then
    '            If foDiscount.HasDiscount Then
    '                Dim lnDisc As Decimal

    '                lnDisc = (foDiscount.Master("nDiscRate") / 100) * foMaster(0).Item("nTranTotl")
    '                lnDisc += foDiscount.Master("nAddDiscx")

    '                foMaster(0).Item("nDiscount") = lnDisc
    '            End If
    '        End If
    '    End If

    '    foMaster(0).Item("nVATSales") = Math.Round((lnTotal - (lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) / lnVatPerc, 2)
    '    foMaster(0).Item("nVATAmtxx") = (lnTotal - (lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) - foMaster(0).Item("nVATSales")

    '    Return True
    'End Function

    'Public Function computeTotal(foMaster As DataTable, _
    '                          foDetail As DataTable, _
    '                          foDtaDisc As DataTable, _
    '                          foDiscount As Discount) As Boolean
    '    Dim lnRow As Integer

    '    Dim lnSlPrc As Double = 0.0

    '    Dim lnDiscntbl As Double = 0.0
    '    Dim lnZeroRatd As Double = 0.0

    '    Dim lnDiscount As Double = 0
    '    Dim lnComplmnt As Double = 0

    '    Dim lnTotlAmnt As Double = 0
    '    Dim lnVoidTotl As Double = 0

    '    For lnRow = 0 To foDetail.Rows.Count - 1
    '        'Is the detail not complement
    '        'Note: complements are not included in our computation
    '        'If foDetail(lnRow).Item("nComplmnt") = 0 Then
    '        'Is the item has a promo?
    '        If IFNull(foDetail(lnRow).Item("nDiscount"), 0) > 0 Or IFNull(foDetail(lnRow).Item("nAddDiscx"), 0) > 0 Then
    '            'Compute for the Total Sale Price
    '            lnSlPrc = (foDetail(lnRow).Item("nUnitPrce") * _
    '                        (100 - foDetail(lnRow).Item("nDiscount")) / 100 -
    '                        foDetail(lnRow).Item("nAddDiscx"))

    '            If foDetail(lnRow).Item("cReversex") = "+" Then
    '                lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '            Else
    '                lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '            End If
    '        Else
    '            'Is there a card discount
    '            If foDtaDisc Is Nothing Then
    '                'No card discount was detected 
    '                'Compute sales price for this item
    '                lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

    '                If foDetail(lnRow).Item("cReversex") = "+" Then
    '                    lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                    lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                Else
    '                    lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                    lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                End If
    '            Else
    '                'A discount card is detected
    '                'Is there a particular category for this discount card
    '                If foDtaDisc(0).Item("sCategrID") = "" Then
    '                    'No particular category
    '                    'Compute sales price for this item
    '                    lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

    '                    If foDetail(lnRow).Item("cReversex") = "+" Then
    '                        lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                        lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                        lnDiscntbl += lnSlPrc
    '                        lnDiscount += (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
    '                    Else
    '                        lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                        lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                        lnDiscntbl -= lnSlPrc
    '                        lnDiscount -= (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
    '                    End If
    '                Else
    '                    'Only a particular category has discount for this card
    '                    Dim loDta() As DataRow
    '                    loDta = foDtaDisc.Select("sCategrID = " & strParm(foDtaDisc(0).Item("sCategrID")))

    '                    'Is the item part of discountable category for this card
    '                    If loDta.Count > 0 Then
    '                        'This is a discountable category for this card
    '                        'Compute sales price for this item
    '                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity")

    '                        Dim lnDiscx As Decimal
    '                        lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nQuantity"))

    '                        If foDetail(lnRow).Item("cReversex") = "+" Then
    '                            lnTotlAmnt += lnSlPrc
    '                            lnDiscount += lnDiscx
    '                            'Compute for compliment here...
    '                            lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
    '                            lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
    '                            lnComplmnt += (lnSlPrc - lnDiscx)
    '                        Else
    '                            lnVoidTotl += lnSlPrc
    '                            lnDiscount -= lnDiscx
    '                            'Compute for compliment here...
    '                            lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
    '                            lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
    '                            lnComplmnt -= (lnSlPrc - lnDiscx)
    '                        End If
    '                    Else
    '                        'This is not discountable category for this card
    '                        'Compute sales price for this item
    '                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

    '                        If foDetail(lnRow).Item("cReversex") = "+" Then
    '                            lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                            lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                        Else
    '                            lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
    '                            lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
    '                        End If

    '                    End If
    '                End If
    '            End If
    '        End If
    '        'kalyptus - 2016.11.28 11:33am
    '        'TODO: Adding of tag whether an ITEM is Vatable or NOT...
    '        'If foDetail(lnRow).Item("cNoneVATx") = "1" Then
    '        '    lnZeroRatd += lnSlPrc
    '        'End If
    '        'End If
    '    Next

    '    foMaster(0).Item("nTranTotl") = lnTotlAmnt
    '    foMaster(0).Item("nDiscntbl") = lnDiscntbl
    '    foMaster(0).Item("nZeroRatd") = lnZeroRatd
    '    foMaster(0).Item("nVoidTotl") = lnVoidTotl
    '    foMaster(0).Item("nDiscount") = 0

    '    Dim lnVatPerc As Double = 1.12

    '    'Compute for a Card Discount without category
    '    If Not foDtaDisc Is Nothing Then
    '        If foDtaDisc(0).Item("sCategrID") = "" Then
    '            Dim lnDisc As Decimal

    '            foDiscount.Master("nDiscRate") = foDtaDisc(0).Item("nDiscRate")
    '            foDiscount.Master("nAddDiscx") = lnDiscount

    '            If foDiscount.Master("cNoneVatx") = "1" Then
    '                'Assume that NON-Vat discount is for SENIOR CITIZEN/PWD card holder only
    '                Dim lnAmtx As Decimal = (lnTotlAmnt - lnVoidTotl - lnComplmnt) / foDiscount.Master("nNoClient") * foDiscount.Master("nWithDisc")
    '                Dim lnNonVat As Decimal

    '                lnNonVat = Math.Round(lnAmtx / lnVatPerc, 2)
    '                foMaster(0).Item("nNonVATxx") = lnAmtx
    '                foMaster(0).Item("nVatDiscx") = lnAmtx - lnNonVat

    '                lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnNonVat
    '                lnDisc += foDtaDisc(0).Item("nDiscAmtx")

    '                foMaster(0).Item("nPWDDiscx") = lnDisc
    '            Else
    '                lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnDiscntbl
    '                lnDisc += foDtaDisc(0).Item("nDiscAmtx")
    '                foMaster(0).Item("nDiscount") = lnDisc
    '            End If
    '        Else
    '            foDiscount.Master("nDiscRate") = 0
    '            foDiscount.Master("nAddDiscx") = lnDiscount
    '            foMaster(0).Item("nDiscount") = lnDiscount
    '        End If
    '    Else
    '        If Not IsNothing(foDiscount) Then
    '            If foDiscount.HasDiscount Then
    '                Dim lnDisc As Decimal

    '                'lnDisc = (foDiscount.Master("nDiscRate") / 100) * (foMaster(0).Item("nTranTotl") - foMaster(0).Item("nVoidTotl") - lnComplmnt)
    '                lnDisc = foDiscount.Master("nDiscRate") * (foMaster(0).Item("nTranTotl") - foMaster(0).Item("nVoidTotl") - lnComplmnt)
    '                lnDisc += foDiscount.Master("nAddDiscx")

    '                foMaster(0).Item("nDiscount") = lnDisc
    '            End If
    '        End If
    '    End If

    '    'Set compliment as discount here...
    '    foMaster(0).Item("nDiscount") += lnComplmnt

    '    'foMaster(0).Item("nVATSales") = Math.Round((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) / lnVatPerc, 2)
    '    'foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) - foMaster(0).Item("nVATSales")

    '    foMaster(0).Item("nVATSales") = Math.Round((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount"))) / lnVatPerc, 2)
    '    foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNlnDiscntblonVATxx") + foMaster(0).Item("nDiscount"))) - foMaster(0).Item("nVATSales")

    '    Return True
    'End Function

    Public Function computeTotal(ByRef foMaster As DataTable,
                                  ByVal foDetail As DataTable,
                                  ByVal foDtaDisc As DataTable,
                                  ByVal foDiscount As Discount) As Boolean
        Dim lnRow As Integer

        Dim lnSlPrc As Double = 0.0

        Dim lnDiscntbl As Double = 0.0
        Dim lnZeroRatd As Double = 0.0

        Dim lnDiscount As Double = 0
        Dim lnComplmnt As Double = 0

        Dim lnTotlAmnt As Double = 0
        Dim lnVoidTotl As Double = 0

        For lnRow = 0 To foDetail.Rows.Count - 1
            'Is the detail not complement
            'Note: complements are not included in our computation
            'If foDetail(lnRow).Item("nComplmnt") = 0 Then
            'Is the item has a promo?
            If IFNull(foDetail(lnRow).Item("nDiscount"), 0) > 0 Or IFNull(foDetail(lnRow).Item("nAddDiscx"), 0) > 0 Then
                'Compute for the Total Sale Price
                lnSlPrc = (foDetail(lnRow).Item("nUnitPrce") *
                            (100 - foDetail(lnRow).Item("nDiscount")) / 100 -
                            (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity")))

                If foDetail(lnRow).Item("cReversex") = "+" Then
                    'lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                    lnTotlAmnt += (foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity"))
                    lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                Else
                    lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                    lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                End If

                lnDiscntbl += (foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity"))
                'lnDiscount += (foDetail(lnRow).Item("nUnitPrce") * _
                '                (foDetail(lnRow).Item("nDiscount") / 100) +
                '                (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity")))

                lnDiscount += ((foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity")) *
                                (foDetail(lnRow).Item("nDiscount") / 100) +
                                (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity")))
            Else
                'Is there a card discount
                If foDtaDisc Is Nothing Then
                    'No card discount was detected 
                    'Compute sales price for this item
                    lnSlPrc = foDetail(lnRow).Item("nUnitPrce")
                    If foDetail(lnRow).Item("cReversex") = "+" Then
                        lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                        lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                    Else
                        lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                        lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                    End If
                Else
                    'A discount card is detected
                    'Is there a particular category for this discount card
                    If foDtaDisc(0).Item("sCategrID") = "" Then
                        'No particular category
                        'Compute sales price for this item
                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

                        If foDetail(lnRow).Item("cReversex") = "+" Then
                            lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            'lnDiscntbl += lnSlPrc
                            lnDiscntbl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnDiscount += (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
                        Else
                            lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            'lnDiscntbl -= lnSlPrc
                            lnDiscntbl -= (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnDiscount -= (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
                        End If
                    Else
                        'Only a particular category has discount for this card
                        Dim loDta() As DataRow
                        loDta = foDtaDisc.Select("sCategrID = " & strParm(foDetail.Rows(lnRow)("sCategrID")))
                        'Is the item part of discountable category for this card
                        If loDta.Count > 0 Then
                            'This is a discountable category for this card
                            'Compute sales price for this item
                            lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity")

                            Dim lnDiscx As Decimal

                            lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nQuantity"))

                            If foDetail(lnRow).Item("cReversex") = "+" Then
                                lnTotlAmnt += lnSlPrc
                                lnDiscount += lnDiscx
                                'Compute for compliment here...
                                lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
                                lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
                                lnComplmnt += (lnSlPrc - lnDiscx)
                            Else
                                lnVoidTotl += lnSlPrc
                                lnDiscount -= lnDiscx
                                'Compute for compliment here...
                                lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
                                lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
                                lnComplmnt -= (lnSlPrc - lnDiscx)
                            End If
                        Else
                            'This is not discountable category for this card
                            'Compute sales price for this item
                            lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

                            If foDetail(lnRow).Item("cReversex") = "+" Then
                                lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                                lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            Else
                                lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                                lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            End If

                        End If
                    End If
                End If
            End If
            'kalyptus - 2016.11.28 11:33am
            'TODO: Adding of tag whether an ITEM is Vatable or NOT...
            'If foDetail(lnRow).Item("cNoneVATx") = "1" Then
            '    lnZeroRatd += lnSlPrc
            'End If
            'End If
        Next

        foMaster(0).Item("nTranTotl") = lnTotlAmnt
        foMaster(0).Item("nDiscntbl") = lnDiscntbl
        foMaster(0).Item("nZeroRatd") = lnZeroRatd
        foMaster(0).Item("nVoidTotl") = lnVoidTotl
        foMaster(0).Item("nDiscount") = 0

        Dim lnVatPerc As Double = 1.12

        'Compute for a Card Discount without category
        If Not foDtaDisc Is Nothing Then
            If foDtaDisc(0).Item("sCategrID") = "" Then
                Dim lnDisc As Decimal

                foDiscount.Master("nDiscRate") = foDtaDisc(0).Item("nDiscRate")
                foDiscount.Master("nAddDiscx") = lnDiscount

                If foDiscount.Master("cNoneVatx") = "1" Then
                    'Assume that NON-Vat discount is for SENIOR CITIZEN/PWD card holder only
                    Dim lnAmtx As Decimal = (lnTotlAmnt - lnVoidTotl - lnComplmnt) / IIf(foDiscount.Master("nNoClient") = 0, 1, foDiscount.Master("nNoClient")) * foDiscount.Master("nWithDisc")
                    Dim lnNonVat As Decimal

                    lnNonVat = Math.Round((lnAmtx / lnVatPerc) + 0.00001, 2)
                    foMaster(0).Item("nNonVATxx") = (lnAmtx / 1.12) + 0.00001
                    'commented this line  it is not necessary to add vat discount if sc
                    'foMaster(0).Item("nVatDiscx") = lnAmtx - lnNonVat

                    lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnNonVat
                    lnDisc += foDtaDisc(0).Item("nDiscAmtx")

                    foMaster(0).Item("nPWDDiscx") = lnDisc
                Else
                    lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * (lnDiscntbl / 1.12)
                    lnDisc += foDtaDisc(0).Item("nDiscAmtx")
                    foMaster(0).Item("nDiscount") = lnDisc
                End If
            Else
                foDiscount.Master("nDiscRate") = 0
                foDiscount.Master("nAddDiscx") = lnDiscount
                foMaster(0).Item("nDiscount") = lnDiscount
            End If
        Else
            If Not IsNothing(foDiscount) Then
                If foDiscount.HasDiscount Then
                    Dim lnDisc As Decimal

                    lnDisc = (IFNull(foDiscount.Master("nDiscRate"), 0) / 100) * (foMaster(0).Item("nTranTotl") - foMaster(0).Item("nVoidTotl") - lnComplmnt)
                    lnDisc += IFNull(foDiscount.Master("nAddDiscx"), 0)

                    foMaster(0).Item("nDiscount") = lnDisc
                End If
            End If
        End If

        'Set compliment as discount here...
        foMaster(0).Item("nDiscount") += lnComplmnt

        'foMaster(0).Item("nVATSales") = Math.Round((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) / lnVatPerc, 2)
        'foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) - foMaster(0).Item("nVATSales")
        Dim xnNonVATxx As Decimal = foMaster(0).Item("nNonVATxx") '
        Dim xnDiscount As Decimal = Math.Round(foMaster(0).Item("nDiscount") + 0.00001, 2)
        Dim xnVATSales As Decimal = foMaster(0).Item("nVATSales")
        Dim xnExmVATxx As Decimal = Math.Round((foMaster(0).Item("nNonVATxx") * 0.12) + 0.00001, 2) '
        foMaster(0).Item("nVATSales") = Math.Round((((lnTotlAmnt - lnVoidTotl) / lnVatPerc) - (lnZeroRatd + xnDiscount + xnNonVATxx)), 2)
        Debug.Print(foMaster(0).Item("nVATSales"))
        'foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + xnDiscount + xnNonVATxx + xnExmVATxx)) - foMaster(0).Item("nVATSales")
        foMaster(0).Item("nVATAmtxx") = Math.Round((foMaster(0).Item("nVATSales") * 0.12) + 0.00001, 2)
        Debug.Print(foMaster(0).Item("nVATAmtxx"))
        '
        '
        '
        '
        '
        '("nvatsales " + Math.Round((lnTotlAmnt / lnVatPerc - (lnVoidTotl + lnZeroRatd + xnNonVATxx + xnDiscount)), 2).ToString)
        'If xnNonVATxx > 0 Then
        '    foMaster(0).Item("nVATAmtxx") = ((lnTotlAmnt / lnVatPerc - (lnVoidTotl + lnZeroRatd + xnNonVATxx + xnDiscount)) - xnVATSales) * 0.12
        '    'MsgBox("nVATAmtxx " + (((lnTotlAmnt / lnVatPerc - (lnVoidTotl + lnZeroRatd + xnNonVATxx + xnDiscount)) - xnVATSales) * 0.12).ToString)
        'Else

        'MsgBox("nVATAmtxx " + ((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + xnNonVATxx + xnDiscount)) - xnVATSales).ToString)
        'End If
        Return True
    End Function

    Public Function computeNeoTotal(ByVal foMaster As DataTable,
                              ByVal foDetail As DataTable,
                              ByVal foDtaDisc As DataTable,
                              ByVal foDiscount As Discount) As Boolean
        Dim lnRow As Integer

        Dim lnSlPrc As Double = 0.0

        Dim lnDiscntbl As Double = 0.0
        Dim lnZeroRatd As Double = 0.0

        Dim lnDiscount As Double = 0
        Dim lnComplmnt As Double = 0

        Dim lnTotlAmnt As Double = 0
        Dim lnVoidTotl As Double = 0

        For lnRow = 0 To foDetail.Rows.Count - 1
            'Is the detail not complement
            'Note: complements are not included in our computation
            'If foDetail(lnRow).Item("nComplmnt") = 0 Then
            'Is the item has a promo?
            If IFNull(foDetail(lnRow).Item("nDiscount"), 0) > 0 Or IFNull(foDetail(lnRow).Item("nAddDiscx"), 0) > 0 Then
                'Compute for the Total Sale Price
                lnSlPrc = (foDetail(lnRow).Item("nUnitPrce") *
                            (100 - foDetail(lnRow).Item("nDiscount")) / 100 -
                            foDetail(lnRow).Item("nAddDiscx"))

                If foDetail(lnRow).Item("cReversex") = "+" Then
                    lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                    lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                Else
                    lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                    lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                End If
            Else
                'Is there a card discount
                If foDtaDisc Is Nothing Then
                    'No card discount was detected 
                    'Compute sales price for this item
                    lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

                    If foDetail(lnRow).Item("cReversex") = "+" Then
                        lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                        lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                    Else
                        lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                        lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                    End If
                Else
                    'A discount card is detected
                    'Is there a particular category for this discount card
                    If foDtaDisc(0).Item("sCategrID") = "" Then
                        'No particular category
                        'Compute sales price for this item
                        lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

                        If foDetail(lnRow).Item("cReversex") = "+" Then
                            lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            lnDiscntbl += lnSlPrc
                            lnDiscount += (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
                        Else
                            lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                            lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            lnDiscntbl -= lnSlPrc
                            lnDiscount -= (foDetail(lnRow).Item("nAddDiscx") * foDetail(lnRow).Item("nQuantity"))
                        End If
                    Else
                        'Only a particular category has discount for this card
                        Dim loDta() As DataRow
                        loDta = foDtaDisc.Select("sCategrID = " & strParm(foDtaDisc(0).Item("sCategrID")))

                        'Is the item part of discountable category for this card
                        If loDta.Count > 0 Then
                            'This is a discountable category for this card
                            'Compute sales price for this item
                            lnSlPrc = foDetail(lnRow).Item("nUnitPrce") * foDetail(lnRow).Item("nQuantity")

                            Dim lnDiscx As Decimal
                            lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nQuantity"))

                            If foDetail(lnRow).Item("cReversex") = "+" Then
                                lnTotlAmnt += lnSlPrc
                                lnDiscount += lnDiscx
                                'Compute for compliment here...
                                lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
                                lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
                                lnComplmnt += (lnSlPrc - lnDiscx)
                            Else
                                lnVoidTotl += lnSlPrc
                                lnDiscount -= lnDiscx
                                'Compute for compliment here...
                                lnSlPrc = foDetail(lnRow).Item("nComplmnt") * foDetail(lnRow).Item("nUnitPrce")
                                lnDiscx = (lnSlPrc * loDta(0).Item("nDiscRate") / 100) + (loDta(0).Item("nDiscAmtx") * foDetail(lnRow).Item("nComplmnt"))
                                lnComplmnt -= (lnSlPrc - lnDiscx)
                            End If
                        Else
                            'This is not discountable category for this card
                            'Compute sales price for this item
                            lnSlPrc = foDetail(lnRow).Item("nUnitPrce")

                            If foDetail(lnRow).Item("cReversex") = "+" Then
                                lnTotlAmnt += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                                lnComplmnt += (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            Else
                                lnVoidTotl += (lnSlPrc * foDetail(lnRow).Item("nQuantity"))
                                lnComplmnt -= (lnSlPrc * foDetail(lnRow).Item("nComplmnt"))
                            End If
                        End If
                    End If
                End If
            End If

            'kalyptus - 2016.11.28 11:33am
            'TODO: Adding of tag whether an ITEM is Vatable or NOT...
            'If foDetail(lnRow).Item("cNoneVATx") = "1" Then
            '    lnZeroRatd += lnSlPrc
            'End If
            'End If
        Next

        foMaster(0).Item("nTranTotl") = lnTotlAmnt
        foMaster(0).Item("nDiscntbl") = lnDiscntbl
        foMaster(0).Item("nZeroRatd") = lnZeroRatd
        foMaster(0).Item("nVoidTotl") = lnVoidTotl
        foMaster(0).Item("nDiscount") = 0

        Dim lnVatPerc As Double = 1.12

        'Compute for a Card Discount without category
        If Not foDtaDisc Is Nothing Then
            If foDtaDisc(0).Item("sCategrID") = "" Then
                Dim lnDisc As Decimal

                foDiscount.Master("nDiscRate") = foDtaDisc(0).Item("nDiscRate")
                foDiscount.Master("nAddDiscx") = lnDiscount

                If foDiscount.Master("cNoneVatx") = "1" Then
                    'Assume that NON-Vat discount is for SENIOR CITIZEN/PWD card holder only
                    Dim lnAmtx As Decimal = (lnTotlAmnt - lnVoidTotl - lnComplmnt) / IIf(foDiscount.Master("nNoClient") = 0, 1, foDiscount.Master("nNoClient")) * foDiscount.Master("nWithDisc")
                    Dim lnNonVat As Decimal

                    lnNonVat = Math.Round(lnAmtx / lnVatPerc, 2)
                    foMaster(0).Item("nNonVATxx") = lnAmtx
                    foMaster(0).Item("nVatDiscx") = lnAmtx - lnNonVat

                    lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnNonVat
                    lnDisc += foDtaDisc(0).Item("nDiscAmtx")

                    foMaster(0).Item("nPWDDiscx") = lnDisc
                Else
                    lnDisc = (foDtaDisc(0).Item("nDiscRate") / 100) * lnDiscntbl
                    lnDisc += foDtaDisc(0).Item("nDiscAmtx")
                    foMaster(0).Item("nDiscount") = lnDisc
                End If
            Else
                foDiscount.Master("nDiscRate") = 0
                foDiscount.Master("nAddDiscx") = lnDiscount
                foMaster(0).Item("nDiscount") = lnDiscount
            End If
        Else
            If Not IsNothing(foDiscount) Then
                If foDiscount.HasDiscount Then
                    Dim lnDisc As Decimal

                    lnDisc = (foDiscount.Master("nDiscRate") / 100) * (foMaster(0).Item("nTranTotl") - foMaster(0).Item("nVoidTotl") - lnComplmnt)
                    lnDisc += foDiscount.Master("nAddDiscx")

                    foMaster(0).Item("nDiscount") = lnDisc
                End If
            End If
        End If

        'Set compliment as discount here...
        foMaster(0).Item("nDiscount") += lnComplmnt

        'foMaster(0).Item("nVATSales") = Math.Round((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) / lnVatPerc, 2)
        'foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount") + foMaster(0).Item("nPWDDiscx") + foMaster(0).Item("nVatDiscx"))) - foMaster(0).Item("nVATSales")

        foMaster(0).Item("nVATSales") = Math.Round((lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount"))) / lnVatPerc, 2)
        foMaster(0).Item("nVATAmtxx") = (lnTotlAmnt - (lnVoidTotl + lnZeroRatd + foMaster(0).Item("nNonVATxx") + foMaster(0).Item("nDiscount"))) - foMaster(0).Item("nVATSales")
        Return True
    End Function

    Enum xeTableStatus
        xeEmpty = 0
        xeOccupied = 1
        xeReserved = 2
        xeDirty = 3
        xeNONE = 4
    End Enum
End Module

