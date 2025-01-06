Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks
Imports MySql.Data.MySqlClient
Public Class GroupeSEIInc
    Inherits System.Web.UI.Page
    Dim data As Data.DataView
    Dim stream_api As String = "GROUPESEI"
    Dim result_api As Boolean = True
#Region "LOAD"
    Sub Load_Search()
        Dim calendarData As DataTable = GetCalendarData()
        Dim pageNumber As Integer = 1 ' The current page number
        Dim recordsPerPage As Integer = 10 ' The number of records per page


        Try
            Dim WhereLine As String = Nothing
            Dim clients_Data As DataTable = GetData("Load_Search", "clients", "", pageNumber, recordsPerPage)
            If clients_Data.Rows.Count > 0 Then
                ViewState("dt") = clients_Data
                rpt_list_client.DataSource = clients_Data
                rpt_list_client.DataBind()

                BindDropDownList("clients", "Search_tb_Clients_category", "Clients_category", WhereLine)
                BindDropDownList("clients", "Search_tb_Clients_groupe", "Clients_groupe", WhereLine)
                BindDropDownList("clients", "Search_tb_Clients_type", "Clients_type", WhereLine)

            End If
        Catch ex As Exception

        End Try

        'Try
        '    Dim WhereLine As String = Nothing
        '    Dim fournisseur_Data As DataTable = GetData("fournisseur", "", pageNumber, recordsPerPage)
        '    If fournisseur_Data.Rows.Count > 0 Then
        '        ViewState("dt") = fournisseur_Data
        '        rpt_list_client.DataSource = fournisseur_Data
        '        rpt_list_client.DataBind()

        '        BindDropDownList("fournisseur", "Search_tb_Fournisseur_category", "Clients_category", WhereLine)
        '        BindDropDownList("fournisseur", "Search_tb_Fournisseur_groupe", "Clients_groupe", WhereLine)
        '        BindDropDownList("fournisseur", "Search_tb_Fournisseur_type", "Clients_type", WhereLine)

        '    End If
        'Catch ex As Exception

        'End Try

        'Try
        '    Dim produits_Data As DataTable = GetData("produits", "", pageNumber, recordsPerPage)
        '    If produits_Data.Rows.Count > 0 Then
        '        ViewState("dt") = produits_Data
        '        rpt_list_produits.DataSource = produits_Data
        '        rpt_list_produits.DataBind()
        '    End If
        'Catch ex As Exception

        'End Try

        'Try
        '    Dim vehicules_Data As DataTable = GetData("vehicules", "", pageNumber, recordsPerPage)
        '    If vehicules_Data.Rows.Count > 0 Then
        '        ViewState("dt") = vehicules_Data
        '        rpt_list_vehicules.DataSource = vehicules_Data
        '        rpt_list_vehicules.DataBind()
        '    End If
        'Catch ex As Exception

        'End Try

        'Try
        '    Dim type_groupe_Data As DataTable = GetData("type_groupe", "", pageNumber, recordsPerPage)
        '    If type_groupe_Data.Rows.Count > 0 Then
        '        ViewState("dt") = type_groupe_Data
        '        rpt_list_montage_sei.DataSource = type_groupe_Data
        '        rpt_list_montage_sei.DataBind()
        '    End If
        'Catch ex As Exception

        'End Try

        'Try
        '    Dim WhereLine As String = Nothing
        '    Dim style_catalogue_Data As DataTable = GetData("style_catalogue", "", pageNumber, recordsPerPage)
        '    If style_catalogue_Data.Rows.Count > 0 Then
        '        ViewState("dt") = style_catalogue_Data
        '        rpt_list_montage.DataSource = style_catalogue_Data
        '        rpt_list_montage.DataBind()

        '        BindDropDownList("style_catalogue", "tb_montage_Prod_groupe_id", "Prod_groupe_id", WhereLine)
        '        BindDropDownList("style_catalogue", "tb_montage_Prod_type_id", "Prod_type_id", WhereLine)
        '        BindDropDownList("style_catalogue", "tb_montage_Prod_categorie_id", "Prod_categorie_id", WhereLine)

        '    End If
        'Catch ex As Exception

        'End Try

        Try
            If calendarData.Rows.Count > 0 Then
                Dim row As DataRow = calendarData.Rows(0)

                ' Assign data to input fields
                asp_settings_general_form_mail_info.Value = row("c_info").ToString()
                asp_settings_general_form_mail_ventes.Value = row("c_ventes").ToString()
                asp_settings_general_form_mail_services.Value = row("c_services").ToString()
                asp_settings_general_form_mail_financement.Value = row("c_financement").ToString()
                asp_settings_general_form_telephone.Value = row("t_info").ToString()
                asp_settings_general_form_delais.Value = row("d_heures").ToString()
                ' Repeat for other fields
                ' ...
            End If
        Catch ex As Exception

        End Try
    End Sub
    Sub Load_Cookie(ByVal Scenario,
                    ByRef Clients_id,
                    ByRef Clients_web,
                    ByRef Fidelity_id,
                    ByRef Query_Langue,
                    ByRef emailid,
                    ByRef securitypin,
                    ByRef semp,
                    ByRef SCompagnie)
        Try
            Client_Cookie(Scenario, "Clients_id", Clients_id)

            'clientsweb_id
            Client_Cookie(Scenario, "Clients_web", Clients_web)


            Client_Cookie(Scenario, "fidelityid", Fidelity_id)

            Client_Cookie(Scenario, "langue", Query_Langue)

            'Clients_email
            Client_Cookie(Scenario, "emailid", emailid)

            Client_Cookie(Scenario, "securitypin", securitypin)

            'clientsweb_semp
            Client_Cookie(Scenario, "semp", semp)

            'clientsweb_scompagnie
            Client_Cookie(Scenario, "SCompagnie", SCompagnie)
        Catch ex As Exception

        End Try

    End Sub
    Sub Load_UI_Cookie(Scenario, Clients_web_step_, ByRef Clients_web_step_value)
        Client_Cookie(Scenario, Clients_web_step_, Clients_web_step_value)

    End Sub
    Sub Client_Cookie(Scenario, Cookies_, ByRef Values_)
        Try
            Select Case True
                Case Scenario = "WRITE"

                    'Create a Cookie with a suitable Key.
                    Dim nameCookie As New HttpCookie(Cookies_)

                    'Set the Cookie value.
                    nameCookie.Values(Cookies_) = Values_

                    ' Définir le chemin du cookie
                    nameCookie.Path = "/"

                    'Set the Expiry date.
                    nameCookie.Expires = DateTime.Now.AddYears(30)

                    ' Ajouter l'attribut Secure pour que le cookie soit uniquement transmis sur HTTPS
                    nameCookie.Secure = True

                    ' Ajouter l'attribut HttpOnly pour que le cookie ne soit pas accessible via JavaScript
                    nameCookie.HttpOnly = True

                    ' Ajouter l'attribut SameSite pour limiter les envois du cookie à des requêtes de navigation de première partie (facultatif)
                    nameCookie.SameSite = SameSiteMode.Strict

                    'Add the Cookie to Browser.
                    Response.Cookies.Add(nameCookie)


                Case Scenario = "LOAD"
                    'Fetch the Cookie using its Key.
                    Values_ = Request.Cookies(Cookies_).Value
                    If Values_ <> Nothing Then
                        Values_ = Values_.ToString.Replace(Cookies_ & "=", "")
                    End If
                Case Else
                    Scenario = Scenario
            End Select

        Catch ex As Exception

        End Try
    End Sub

    Sub Show_Online_Client(Clients_id_, Fidelity_id, ByRef Clients_comp, ByRef Clients_addr_1)
        Dim Scenario_ = Nothing, Clients_contact = Nothing, Clients_admin = Nothing,
          Clients_groupe = Nothing, Clients_email = Nothing, Clients_famille = Nothing, Clients_type = Nothing,
                    Clients_category = Nothing, Clients_cp_1 = Nothing, Clients_ville_1 = Nothing,
                    Clients_prov_1 = Nothing, Clients_pays_1 = Nothing, Clients_phone_contact = Nothing, Clients_telcel = Nothing,
      Clients_territoire = Nothing, Clients_fax = Nothing, Clients_birthday = Nothing, Clients_genre = Nothing, Clients_sexe = Nothing, Clients_autrea = Nothing
        FindClientInfo(connecStr, Clients_id_, Clients_comp, Clients_contact, Clients_groupe, Clients_famille, Clients_type,
                              Clients_category, Fidelity_id, Clients_birthday, Clients_admin, Clients_addr_1, Clients_cp_1, Clients_ville_1,
                              Clients_prov_1, Clients_pays_1, Clients_phone_contact, Clients_telcel, Clients_genre, Clients_sexe, Clients_autrea, Clients_territoire,
                              Clients_email)
        btn_Signup_Confirm_member.InnerText = "Sauvegarder"
        Try
            Load_Search()
            Title_Manager_name_profil.InnerText = Clients_comp
            client_candidats_card_statut.InnerText = Clients_contact
            Select Case True
                Case Clients_autrea <> Nothing
                    btn_client_membre_card.Visible = True
                    Manager_Login_MemberId.InnerText = Clients_autrea
                    btn_Panel_Parametre_Online_Menu_Utilitaire.Visible = True
                    btn_Panel_Parametre_Online_Menu_Operations.Visible = True
                    btn_Panel_Parametre_Online_Menu_Liste.Visible = True
            End Select
            Manager_Login_Telephone.InnerText = Clients_telcel
            Manager_Login_Email.InnerText = Clients_email
            Manager_Login_Circonscription.InnerText = Clients_type
            Manager_Login_Clients_addr_1.InnerText = Clients_addr_1
            Manager_Login_Clients_ville_1.InnerText = Clients_ville_1
            Manager_Login_Clients_cp_1.InnerText = Clients_cp_1


            Select Case True
                Case Clients_pays_1 <> Nothing
                    Manager_Login_Clients_pays_1.InnerText = Clients_pays_1

                Case Else
                    Manager_Login_Clients_pays_1.InnerText = "Canada"

            End Select
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Load_Scenario"
    Protected Sub btn_recherche_produit_back_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_recherche_produit_back_ServerClick")
    End Sub
    Protected Sub btn_Panel_Entrerdespieces_Search_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Entrerdespieces_Search_ServerClick")
    End Sub
    Sub Load_Scenario_Panel_Menu_Online()
        Panel_Menu_Taches.Visible = True
        Panel_Menu_Taches_Online_1.Visible = True
        btn_Panel_CreationBT.Visible = True
    End Sub
    Sub Load_Scenario_Panel_Menu_Offline()
        Panel_Menu_Taches.Visible = True
        Panel_Menu_Taches_Offline_1.Visible = True
        Panel_Menu_Taches_Online_1.Visible = False
        btn_Panel_CreationBT.Visible = False

    End Sub
    Protected Sub btn_Panel_MenuWeb_Montage_Web_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_MenuWeb_Montage_Web_ServerClick")
    End Sub
    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Load_Scenario("Page_Load")
        End If
    End Sub
    Protected Sub btn_Panel_Menu_Taches_Offline_1_Menu_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Menu_Taches_Offline_1_Menu_ServerClick")

    End Sub
    Protected Sub btn_log_out_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("LOG_OFF")
    End Sub
    Protected Sub btn_SortirdeJob_ServerClick(sender As Object, e As EventArgs) Handles btn_SortirdeJob.ServerClick
        Dim stg_ As String = lbl_btn_SortirdeJob.InnerText
        Select Case True
            Case stg_ = "Entrée"
                Load_Scenario("btn_EntreedeJob_ServerClick")
            Case Else
                SortirJob_IDBondetravail_Q1.Visible = True
        End Select
    End Sub

    Protected Sub SortirJob_IDBondetravail_Q1_NO_ServerClick(sender As Object, e As EventArgs)
        SortirJob_IDBondetravail_Q1.Visible = False
        SortirJob_IDBondetravail_Q2.Visible = True
    End Sub
    Protected Sub btn_Panel_PunchInOut_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_PunchInOut_ServerClick")
    End Sub
    Protected Sub btn_Panel_Montage_WEN_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Montage_WEN_ServerClick")

    End Sub
    Protected Sub btn_BT_en_cours_employe_complet_ServerClick1(sender As Object, e As EventArgs)
        Load_Scenario("btn_BT_en_cours_employe_complet_ServerClick1")
    End Sub
    Protected Sub btn_wo_actuel_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_wo_actuel_ServerClick")
    End Sub
    Protected Sub btn_wo_actuel_Agenda_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_wo_actuel_Agenda_ServerClick")
    End Sub

    Protected Sub btn_Panel_details_emp_2_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_details_emp_2_ServerClick")
    End Sub
    Protected Sub btn_Panel_details_emp_3_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_details_emp_3_ServerClick")
    End Sub
    Protected Sub btn_Panel_Entrerdestemps_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Entrerdestemps_ServerClick")
    End Sub
    Protected Sub btn_Panel_Entrerdespieces_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Entrerdespieces_ServerClick")
    End Sub

    Protected Sub btn_Panel_Bontravail_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Bontravail_ServerClick")
    End Sub
    Protected Sub btn_Panel_Entrerdestravails_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Entrerdestravails_ServerClick")
    End Sub
    Protected Sub btn_Panel_Detail_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Detail_ServerClick")
    End Sub
    Protected Sub btn_wo_complet_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_wo_complet_ServerClick")
    End Sub
    Protected Sub btn_Panel_CreationJOBS_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_CreationJOBS_ServerClick")
    End Sub
    Protected Sub btn_Panel_CreationBT_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_CreationBT_ServerClick")
    End Sub
    Protected Sub btn_Filter_Letter_Parts_filter_jobs_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("Filter_Letter_Parts")
    End Sub
    Protected Sub btn_Filter_Letter_Parts_reset_ServerClick(sender As Object, e As EventArgs)
        tb_Panel_Entrerdespieces_Description.Text = Nothing
        Load_Scenario("Filter_Letter_Parts")
    End Sub

    Protected Sub btn_Filter_Letter_Parts_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("Filter_Letter_Parts")
    End Sub

    Protected Sub btn_Filter_Letter_Tasks_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("Filter_Letter_Tasks")
    End Sub
    Protected Sub btn_Panel_Montage_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Montage_ServerClick")
    End Sub
    Protected Sub dp_Filter_Letter_Tasks_reset_ServerClick(sender As Object, e As EventArgs)
        dp_Filter_Letter_Tasks.Text = "Choisir une lettre"
        Load_Scenario("Filter_Letter_Tasks")

    End Sub

    Protected Sub tb_Panel_Entrerdestravails_Description_reset_ServerClick(sender As Object, e As EventArgs)
        tb_Panel_Entrerdestravails_Description.Text = ""
        Load_Scenario("Filter_Letter_Tasks")

    End Sub

    Protected Sub btn_Panel_details_BonTravails_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_details_BonTravails_ServerClick")
    End Sub
    Protected Sub SortirJob_IDBondetravail_Q2_Break_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("SortirJob_IDBondetravail_Q2_Break_ServerClick")
    End Sub

    Protected Sub SortirJob_IDBondetravail_Q2_Close_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("SortirJob_IDBondetravail_Q2_Close_ServerClick")
    End Sub

    Protected Sub SortirJob_IDBondetravail_Q2_Continue_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("SortirJob_IDBondetravail_Q2_Continue_ServerClick")
    End Sub

    Protected Sub btn_offline_panel_AllJobs_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_offline_panel_AllJobs_ServerClick")
    End Sub
    Protected Sub btn_offline_panel_Jobs_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_offline_panel_Jobs_ServerClick")
    End Sub
    Protected Sub btn_offline_panel_Agenda_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_offline_panel_Agenda_ServerClick")
    End Sub
    Protected Sub A36_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("Page_Load")
    End Sub
    Protected Sub btn_Agenda_Portail_refresh_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Agenda_Portail_refresh_ServerClick")
    End Sub

#End Region

#Region "NAVIGATION BUTTON"
#Region "UI"
    Sub Reset_Sub_nav()
        Reset_Panel()
        Reset_main_panel()
        Reset_Sign_up()
    End Sub
    Sub Reset_All()
        Reset_Sub_nav()
    End Sub







    Sub Reset_main_panel()
        'Flex_logo.Visible = False
        Offline_Panel.Visible = False
        Level2.Visible = False
    End Sub
    Sub Reset_panel_APP()
        Panel_Choix_Licence.Visible = False
        Panel_Choix_Email.Visible = False
        Panel_Choix_Compagnie.Visible = False
        Panel_Choix_Password.Visible = False
        Creer_Usager_Alert.Visible = False
    End Sub
    Sub Reset_Sign_up()
        Signup_step_member.Visible = False
        Condition_Utilisation.Visible = False
        Signup.Visible = False

    End Sub
    Sub Main_Level_2()
        Reset_All()
        Level2.Visible = True
        Condition_Utilisation.Visible = True
    End Sub


    Sub Go_LIVE_Citoyen()
        Reset_Sub_nav()
    End Sub
    Sub Go_LIVE_Manager()
        Reset_Sub_nav()
    End Sub

#End Region
#Region "STEP"

    Sub Step_Email()
        Reset_panel_APP()
        Load_Cookie("WRITE", "", "", "", "", "", "", "", "")
        Panel_Choix_Email.Visible = True
    End Sub
    Sub Step_Sign_up_1()
        Reset_Sign_up()
        Signup.Visible = True
        Signup_step_member_up.Focus()
    End Sub
    'Sub Step_Sign_up_2_Body(Clients_email, Fidelity_id)
    '    Sign_up_2_Final_Step(Clients_email, Fidelity_id)
    '    Reset_Sign_up()
    '    Signup_step_member.Visible = True
    '    Creer_Usager_Alert.Visible = False
    'End Sub
    Sub Step_Sign_up_2()
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing,
            semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)

        Dim ready_ As Boolean = False
        Creer_Usager_Alert_Message.InnerHtml = ""
        If tb_fname.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le prenom"
        End If
        If tb_lname.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le nom de famille"
        End If
        'If tb_phone.Value = "" Then
        '    ready_ = True
        '    Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le telephone"
        'End If
        If tb_email.Value = "" And Clients_email = Nothing Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le courriel"
        End If
        If db_check.Checked = False Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque l'autorisation de recevoir des notifications"
        End If
        'If DropDownList_Communication.Text = "Choisir Type" Then
        '    ready_ = True
        '    Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque votre preference de correspondance"
        'End If
        'If db_Objectif.Text = "Choisir Objectif" Then
        '    ready_ = True
        '    Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque votre objectif"
        'End If
        If ready_ <> True Then
            Dim Clients_comp As String = tb_lname.Value & "," & tb_fname.Value
            If Fidelity_id = Nothing Then
                Fidelity_id = tb_password.Value
            End If

            Dim DBSelection As String = "clients"
            Dim FieldVariable_ As String = "Clients_id"
            Where_To_Update(DBSelection, FieldVariable_, Clients_id_, Clients_comp, "Clients_comp", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id_, tb_phone.Value, "Clients_telcel", DefaultDataBase)
            Reset_Sign_up()
            Signup_step_member.Visible = True
            Creer_Usager_Alert.Visible = False
        Else
            Creer_Usager_Alert.Visible = True
        End If
    End Sub
    Sub Step_Sign_up_3()

        Dim Clients_id As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)

#Region "VARIQABLE"

        Dim ready_ As Boolean = False
        Creer_Usager_Alert_Message.InnerHtml = ""
        'If Signup_step_code.Value = "" Then
        '    ready_ = True
        '    Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le code de verification recu par courriel"
        'Else
        '    Fidelity_id = Signup_step_code.Value
        '    If Find_if_Client_Exist(Clients_id_, Fidelity_id, "", "") = False Then
        '        ready_ = True
        '        Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Le code de verification recu par courriel ne correspond pas au compte"
        '    End If
        'End If

        If Signup_step_member_Clients_addr_1.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le addresse"
        End If

        If Signup_step_member_Clients_ville_1.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le ville"
        End If
        If Signup_step_member_Clients_cp_1.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le code postal"
        End If
        If Signup_step_member_Clients_type.Text = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque la circonscription"
        End If

        'If Signup_step_member_Pays.Text = "" Then
        '    ready_ = True
        '    Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque la circonscription"
        'End If

        If Signup_step_member_Langue.Text = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque votre preference de correspondance"
        End If
        If Signup_step_timezone.Value = "" Then
            ready_ = True
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque votre fuseau horaire"
        End If
#End Region

        Select Case True
            Case ready_ = True
                Creer_Usager_Alert.Visible = True
                Exit Sub
            Case Else
                Clients_id = Creer_Client(Clients_id, Fidelity_id, Clients_email, semp, SCompagnie)
                Go_LIVE_Citoyen()
        End Select
        Creer_Usager_Alert.Visible = False




    End Sub
#End Region
    Sub Reset_Panel()

        Panel_Repeater_Metronic_Btemploye_M.Visible = False
        Panel_Repeater_Metronic_Btemploye_M_Jobs.Visible = False

        Panel_MontageWeb.Visible = False
        recherche_produit.Visible = False

        Panel_Repeater_wo_complet_Load_Jobs.Visible = False
        Panel_Repeater_wo_complet.Visible = False

        kt_tab_pane_seipunch_alert.Visible = False

        Panel_PreFacture.Visible = False
        Panel_Customer.Visible = False
        Panel_Repeater_Metronic_allWO_Load_Jobs.Visible = False
        Panel_Repeater_Metronic_allWO.Visible = False

        Panel_Taches_Entrerdespieces.Visible = False
        Panel_Taches_Entrerdestravails.Visible = False
        Panel_Menu_Taches.Visible = False
        Panel_Entrerdestravails_alert.Visible = False
        Panel_Entrerdestravails_success.Visible = False
        Panel_Entrerdespieces_alert.Visible = False
        Panel_Entrerdespieces_success.Visible = False
        Panel_Entrerdestemps.Visible = False
        Panel_Entrerdespieces.Visible = False
        Panel_Entrerdestravails.Visible = False
        Panel_CreationBT.Visible = False
        Panel_TreeView_kt_modal_allWO.Visible = False
        Panel_TreeViewBtemploye_M.Visible = False

        Offline_agenda_actuel.Visible = False

        agenda_actuel.Visible = False
        wo_actuel.Visible = False
        wo_complet.Visible = False
        Panel_CreationJOBS.Visible = False
        Panel_details_veh_detail.Visible = False
        Panel_details_cli_detail.Visible = False
        Panel_Fiche_Produit.Visible = False
        Panel_Fiche_Vehicules.Visible = False
    End Sub
    Sub Licence_Reaction(Fidelity_id)
        Creer_Usager_Alert.Visible = True
        Select Case True
            Case Fidelity_id = "VIDE"
                Creer_Usager_Alert_Message.InnerText = "Vous n'avez pas mis de courriel !"
            Case Fidelity_id = "ERREUR"
                Creer_Usager_Alert_Message.InnerText = "Vous n'etes pas membre a ce courriel, veuillez devenir membre !"
            Case Fidelity_id = "COMP-1000"
                Creer_Usager_Alert_Message.InnerText = "La compagnie n'a plus de licence libre"
            Case Fidelity_id = "COMP-1001"
                Creer_Usager_Alert_Message.InnerText = "Vous devez choisir une compagnie"
            Case Fidelity_id = "EMP-1000"
                Creer_Usager_Alert_Message.InnerText = "Vote profil n'a plus de licence libre, nous avons donc Remis à Zéro votre profil"

        End Select
    End Sub

#Region "EMAIL"
    Protected Sub Email_Panel_button_btn_ServerClick(sender As Object, e As EventArgs)
        Select Case True
            Case tb_email.Value = Nothing
                Licence_Reaction("VIDE")
            Case Else
                Step_Email(tb_email.Value)
        End Select
    End Sub
    Sub Step_Email(Clients_email)

        Dim Clients_web_step1 As Boolean = check_Clients_web_step1.Checked
        If Clients_web_step1 = True Then
            Load_UI_Cookie("WRITE", "Clients_web_step1", Clients_email)
        End If

        Try
            Dim compagnieweb_e As String = Nothing
            ddl_Panel_MenuWeb_Compagnie.Items.Clear()
            ddl_Panel_MenuWeb_Compagnie.Items.Add("Choisir Compagnie")
            Dim Get_Data_Compagnie As DataTable = GetData("", "clientsweb", " WHERE Clients_email = '" & Clients_email & "'", 1, 10000)
            For Each row As DataRow In Get_Data_Compagnie.Rows
                Dim compagnieweb_index As String = row("compagnieweb_index").ToString.Trim
                Dim clientsweb_scompagnie As String = GetColumnValueFromDB("compagnieweb", "compagnieweb_index", compagnieweb_index, "compagnieweb_id", "GSIGSI")
                Dim nom As String = GetColumnValueFromDB("compagnieweb", "compagnieweb_index", compagnieweb_index, "compagnieweb_descr", "GSIGSI")
                If nom.Contains("/") Then
                    nom = nom.Replace("/", "")
                End If
                compagnieweb_e = nom & " / " & clientsweb_scompagnie
                ddl_Panel_MenuWeb_Compagnie.Items.Add(compagnieweb_e)
            Next
        Catch ex As Exception

        End Try


        Dim Clients_web_step2 As String
        Dim Clients_web_step3 As String
        Dim Clients_web_step4 As String
        Dim Clients_web_step5 As String
        Try
            Load_UI_Cookie("LOAD", "Clients_web_step2", Clients_web_step2)

            Load_UI_Cookie("LOAD", "Clients_web_step3", Clients_web_step3)

            Load_UI_Cookie("LOAD", "Clients_web_step4", Clients_web_step4)

            Load_UI_Cookie("LOAD", "Clients_web_step5", Clients_web_step5)

            Dim compagnieweb_index As String
            Load_UI_Cookie("LOAD", "compagnieweb_index", compagnieweb_index)

            Client_Cookie("WRITE", "emailid", Clients_email)

            Dim ProductToShow_ As Integer = Process_Count_Mysql("", "clientsweb_key", " WHERE clientsweb_id = '" & Clients_email & "' AND clientsweb_key = '" & Clients_web_step5 & "'")

            Select Case True
                Case ProductToShow_ = 0

                    ddl_Panel_MenuWeb_Compagnie.Text = "Choisir Compagnie"

                Case Else

                    If Clients_web_step2 <> Nothing And compagnieweb_index <> Nothing And Clients_web_step5 <> Nothing Then
                        Reset_panel_APP()
                        If Clients_web_step3 = True Then
                            Load_UI_Cookie("WRITE", "Fidelity_id", Clients_web_step5)
                            Load_UI_Cookie("WRITE", "securitypin", Clients_web_step5)

                            Load_Cookie_UI(Clients_web_step3, Clients_web_step4, Clients_email, compagnieweb_index, Clients_web_step5)
                        Else
                            ddl_Panel_MenuWeb_Compagnie.Text = Clients_web_step2
                            check_Clients_web_step2.Checked = True
                        End If
                        Exit Sub
                    End If

                    If Clients_web_step2 <> Nothing Then
                        ddl_Panel_MenuWeb_Compagnie.Text = Clients_web_step2
                        check_Clients_web_step2.Checked = True
                    Else
                        ddl_Panel_MenuWeb_Compagnie.Text = "Choisir Compagnie"
                    End If
            End Select

        Catch ex As Exception

        End Try

        Reset_panel_APP()
        Panel_Choix_Compagnie.Visible = True
    End Sub
#End Region

#Region "COMPAGNIE"
    Protected Sub btn_Panel_Choix_Compagnie_next_ServerClick(sender As Object, e As EventArgs)
        Dim compagnieweb_id As String = ddl_Panel_MenuWeb_Compagnie.Text
        If ddl_Panel_MenuWeb_Compagnie.Text = "Choisi compagnie" Then
            Licence_Reaction("COMP-1001")
            Exit Sub
        End If

        Dim Clients_web_step2 As String = check_Clients_web_step2.Checked.ToString
        If Clients_web_step2 = "True" Then
            Load_UI_Cookie("WRITE", "Clients_web_step2", ddl_Panel_MenuWeb_Compagnie.Text)
        End If

        compagnieweb_id = compagnieweb_id.Substring(compagnieweb_id.Length() - 6, 6)
        Dim compagnieweb_index As String = GetColumnValueFromDB("compagnieweb", "compagnieweb_id", compagnieweb_id, "compagnieweb_index", "GSIGSI")
        Dim Fidelity_id As String
        Dim Clients_email As String = tb_email.Value
        Dim clientsweb_licence As Integer = GetColumnValueFromDB("clientsweb", "compagnieweb_index", compagnieweb_index, "clientsweb_licence", "GSIGSI")
        Dim clientsweb_status As String = GetColumnValueFromDB("clientsweb", "compagnieweb_index", compagnieweb_index, "clientsweb_status", "GSIGSI")

        Dim stg_whereline As String = " WHERE compagnieweb_index = '" & compagnieweb_index & "' AND clientsweb_id = '" & Clients_email & "'"
        Dim Count_ As Integer = Process_Count_Mysql("", "clientsweb_key", stg_whereline)
        Dim clientsweb_id As String = GetData_Distinct_String("clientsweb", "clientsweb_id", stg_whereline)
        Try


            Reset_panel_APP()
            Select Case True
                Case Count_ > clientsweb_licence Or Count_ > 0
                    Select Case True
                        Case Count_ > clientsweb_licence
                            Select Case True
                                Case (clientsweb_licence - Count_) = 0
                                    tb_Panel_Choix_Licence.Value = (clientsweb_licence - Count_).ToString & " Licence"
                                Case Else
                                    tb_Panel_Choix_Licence.Value = (clientsweb_licence - Count_).ToString & " Licences"

                            End Select
                            'SI TU DEPASSE TON MEMBERSHIP ON TE RESET
                            DeleteCustomerByName("clientsweb_key", " WHERE clientsweb_id = '" & Clients_email & "' AND compagnieweb_index = '" & compagnieweb_index & "'")
                            Where_To_Update("clientsweb", "clientsweb_id", clientsweb_id, 1, "clientsweb_licence_status", "gsi")
                        Case Else
                            'Where_To_Update("clientsweb", "clientsweb_id", clientsweb_id, Count_, "clientsweb_licence_status", "gsi")
                    End Select

                    Dim Clients_web_step3 As String
                    Load_UI_Cookie("LOAD", "Clients_web_step3", Clients_web_step3)

                    Dim Clients_web_step4 As String
                    Load_UI_Cookie("LOAD", "Clients_web_step4", Clients_web_step4)

                    Dim Clients_web_step5 As String
                    Load_UI_Cookie("LOAD", "Clients_web_step5", Clients_web_step5)

                    If Clients_web_step5 = Nothing Then
                        Clients_web_step5 = GetData_Distinct_String("clientsweb_key", "clientsweb_key", stg_whereline)
                        Load_UI_Cookie("WRITE", "Fidelity_id", Clients_web_step5)
                        Load_UI_Cookie("WRITE", "securitypin", Clients_web_step5)
                        Load_UI_Cookie("WRITE", "Clients_web_step5", Clients_web_step5)
                    End If
                    Load_Cookie_UI(Clients_web_step3, Clients_web_step4, Clients_email, compagnieweb_index, Clients_web_step5)

                Case clientsweb_status = "ACTIVE"
                    Fidelity_id = Step_Panel_MenuWeb_Scenario(Clients_email, compagnieweb_index)
                    Step_Choix_Compagnie(Clients_email, compagnieweb_index, Fidelity_id)

                Case Else
                    clientsweb_id = clientsweb_id
            End Select

        Catch ex As Exception

        End Try


    End Sub

    Function Step_Panel_MenuWeb_Scenario(Clients_email, compagnieweb_index) As String
        Dim stg_whereline As String = " WHERE compagnieweb_index = '" & compagnieweb_index & "' AND clientsweb_id = '" & Clients_email & "'"
        Dim Fidelity_id As String = GetData_Distinct_String("clientsweb_key", "clientsweb_key", stg_whereline)

        stg_whereline = " WHERE compagnieweb_index = '" & compagnieweb_index & "' AND Clients_email = '" & Clients_email & "'"
        Dim semp As String = GetData_Distinct_String("clientsweb", "clientsweb_semp", stg_whereline)
        Dim clientsweb_id As String = GetData_Distinct_String("clientsweb", "clientsweb_id", stg_whereline)
        Client_Cookie("WRITE", "semp", semp)

        If Fidelity_id = Nothing Then
            Fidelity_id = random.Next(100000, 999999).ToString()

            Dim clientsweb_key_index As Integer = Find_Next_Licence_Entry()
            Dim DBBase As String = "gsi"
            Where_To_Update("clientsweb_key", "clientsweb_key_index", clientsweb_key_index, Fidelity_id, "clientsweb_key", DBBase)
            Where_To_Update("clientsweb_key", "clientsweb_key_index", clientsweb_key_index, Clients_email, "clientsweb_id", DBBase)
            Where_To_Update("clientsweb_key", "clientsweb_key_index", clientsweb_key_index, compagnieweb_index, "compagnieweb_index", DBBase)
            'Where_To_Update("clientsweb", "clientsweb_id", clientsweb_id, 1, "clientsweb_licence_status", "gsi")

        End If
        Client_Cookie("WRITE", "emailid", Clients_email)
        Client_Cookie("WRITE", "securitypin", Fidelity_id)
        Dim Clients_id_ As String = Find_Client_ID_From_Clients_email(Clients_email)
        Client_Cookie("WRITE", "Clients_id", Clients_id_)
        Client_Cookie("WRITE", "SCompagnie", compagnieweb_index)



        Return Fidelity_id
    End Function
    Sub Step_Choix_Compagnie(Clients_email, compagnieweb_index, Fidelity_id)
        Licence_Step_6(Clients_email, Fidelity_id)
        Panel_Choix_Password.Visible = True
    End Sub
    Protected Sub btn_Panel_Choix_Compagnie_back_ServerClick(sender As Object, e As EventArgs)
        Reset_panel_APP()
        Panel_Choix_Email.Visible = True

    End Sub
#End Region


#Region "LICENCE"
    Protected Sub btn_Panel_Choix_Licence_back_Click(sender As Object, e As EventArgs)
        Reset_panel_APP()
        Panel_Choix_Compagnie.Visible = True
    End Sub

    Protected Sub btn_Panel_Choix_Licence_next_Click(sender As Object, e As EventArgs)
        Dim Clients_web As String = Nothing,
       Clients_id As String = Nothing,
       Fidelity_id As String = Nothing,
       Query_Langue As String = Nothing,
       Clients_email As String = Nothing,
       semp As String = Nothing,
       SCompagnie As String = Nothing,
       securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)

        Dim Clients_web_step3 As String = check_Clients_web_step3.Checked.ToString
        If Clients_web_step3 = "True" Then
            Load_UI_Cookie("WRITE", "Clients_web_step3", Clients_web_step3)
        End If


        Dim stg_whereline As String = " WHERE compagnieweb_index = '" & SCompagnie & "' AND Clients_email = '" & Clients_email & "'"
        Dim clientsweb_id As String = GetData_Distinct_String("clientsweb", "clientsweb_id", stg_whereline)

        DeleteCustomerByName("clientsweb_key", " WHERE clientsweb_id = '" & Clients_email & "' AND compagnieweb_index = '" & SCompagnie & "'")


        Reset_panel_APP()

        'Dim clientsweb_licence_status As Integer = 0
        'Where_To_Update("clientsweb", "clientsweb_id", clientsweb_id, clientsweb_licence_status, "clientsweb_licence_status", "gsi")


        Fidelity_id = Step_Panel_MenuWeb_Scenario(Clients_email, SCompagnie)
        Step_Choix_Compagnie(Clients_email, SCompagnie, Fidelity_id)
        'Load_Panel_MenuWeb_Compagnie(Clients_email, SCompagnie)



    End Sub
#End Region

#Region "PASSWORD"
    Protected Sub btn_Panel_Choix_Password_back_ServerClick(sender As Object, e As EventArgs)
        Reset_panel_APP()
        Panel_Choix_Email.Visible = True
    End Sub
    Protected Sub Password_Panel_button_btn_ServerClick(sender As Object, e As EventArgs)
        Dim Fidelity_id As String = tb_password.Value
        Dim Clients_web As String = Nothing,
       Query_Langue As String = Nothing,
       Clients_email As String = Nothing,
       semp As String = Nothing,
       SCompagnie As String = Nothing,
       securitypin As String = Nothing
        Load_Cookie("LOAD", "", "", "", Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim ready_ As Boolean = True
        Creer_Usager_Alert_Message.InnerHtml = ""

        If Fidelity_id = "" Then
            ready_ = False
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Il manque le code de verification recu par courriel"
        ElseIf Fidelity_id <> securitypin Then
            ready_ = False
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Le code de verification n'est pas correct"
        End If

        Dim Clients_web_step4 As String = check_Clients_web_step4.Checked.ToString
        If Clients_web_step4 = "True" Then
            Load_UI_Cookie("WRITE", "Clients_web_step4", Clients_web_step4)
            Load_UI_Cookie("WRITE", "Clients_web_step5", tb_password.Value)
        End If

        If ready_ = True Then
            Step_Password(Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Else
            Creer_Usager_Alert.Visible = True
        End If

    End Sub
    Sub Step_Password(Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim Clients_id As String = Find_Client_ID_From_Clients_email(Clients_email)
        Select Case True
            Case Clients_id = Nothing
                Clients_id = Creer_Client(Clients_id, Fidelity_id, Clients_email, semp, SCompagnie)

            Case Else
                Dim DBSelection As String = "clients"
                Dim FieldVariable_ As String = "Clients_id"
                Where_To_Update(DBSelection, FieldVariable_, Clients_id, Fidelity_id, "Fidelity_id", DefaultDataBase)
        End Select

        Clients_web = GetColumnValueFromDB("clients", "Clients_id", Clients_id, "Clients_noclient", "GSIGSI")
        semp = GetColumnValueFromDB("clientsweb", "clientsweb_id", Clients_web, "clientsweb_semp", "GSIGSI")
        SCompagnie = GetColumnValueFromDB("clientsweb", "clientsweb_id", Clients_web, "compagnieweb_index", "GSIGSI")

        Load_UI_Cookie("WRITE", "compagnieweb_index", SCompagnie)

        Load_Cookie("WRITE", Clients_id, Clients_web, Fidelity_id, "", Clients_email, securitypin, semp, SCompagnie)
        'Online_Client(Clients_id, Fidelity_id)
        Try
            Response.Redirect("GroupeSEIInc.aspx", True)
        Catch ex As ThreadAbortException
        Catch ex As Exception
            Creer_Usager_Alert_Message.InnerHtml += "<br />" & "Veuillez nous joindre au 450-700-0266"
        End Try

    End Sub
    Sub Load_Cookie_UI(Clients_web_step3, Clients_web_step4, Clients_email, compagnieweb_index, Fidelity_id)
        If Clients_web_step3 = "" Then
            'Licence_Reaction("EMP-1000")
            Panel_Choix_Licence.Visible = True
        Else
            If Clients_web_step4 = "" Then
                Step_Choix_Compagnie(Clients_email, compagnieweb_index, Fidelity_id)
            Else
                If Fidelity_id <> Nothing Then
                    Dim Clients_id As String = Find_Client_ID_From_Clients_email(Clients_email)
                    Dim Clients_web As String = GetColumnValueFromDB("clients", "Clients_id", Clients_id, "Clients_noclient", "GSIGSI")
                    Dim stg_whereline As String = " WHERE compagnieweb_index = '" & compagnieweb_index & "' AND Clients_email = '" & Clients_email & "'"
                    Dim semp As String = GetData_Distinct_String("clientsweb", "clientsweb_semp", stg_whereline)

                    Step_Password(Clients_web, Fidelity_id, "", Clients_email, Fidelity_id, semp, compagnieweb_index)
                Else
                    Panel_Choix_Password.Visible = True
                    check_Clients_web_step4.Checked = True
                End If

            End If

        End If
    End Sub
#End Region

    Protected Sub btn_condition_utilisation_ServerClick(sender As Object, e As EventArgs)

        Step_Sign_up_1()
    End Sub
    Protected Sub btn_Signup_Confirm_ServerClick(sender As Object, e As EventArgs)
        Step_Sign_up_2()
    End Sub
    Protected Sub btn_Signup_Cancel_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing,
            semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)

        Select Case True
            Case Process_Count_Mysql("", "clients", " WHERE Clients_id = '" & Clients_id_ & "' AND Fidelity_id = '" & Fidelity_id & "' AND Clients_Plateforme = '" & UMan_API_Name & "'") = 0
                Reset_Sign_up()
                Condition_Utilisation.Visible = True
            Case Else

        End Select
    End Sub
    Protected Sub btn_Signup_Confirm_member_ServerClick(sender As Object, e As EventArgs)
        Step_Sign_up_3()
    End Sub
    Protected Sub btn_Signup_Cancel_member_ServerClick(sender As Object, e As EventArgs)
        Step_Sign_up_1()
    End Sub
    Protected Sub btn_Signup_Confirm_api_ServerClick(sender As Object, e As EventArgs)
        Step_Sign_up_1()
    End Sub
    Protected Sub btn_back_to_step_1_ServerClick(sender As Object, e As EventArgs)
        Step_Email()
    End Sub
    Protected Sub btn_condition_utilisation_cancel_ServerClick(sender As Object, e As EventArgs)
        Reset_All()

        Reset_Sign_up()
        Panel_Choix_Email.Visible = True

    End Sub
    Protected Sub btn_edit_profil_ServerClick(sender As Object, e As EventArgs)
        Reset_All()
        Level2.Visible = True
        Signup.Visible = True
    End Sub
    Protected Sub level_sub_nav_3_ServerClick(sender As Object, e As EventArgs)
        Go_LIVE_Citoyen()
    End Sub
    Protected Sub level_sub_nav_2_ServerClick(sender As Object, e As EventArgs)
        Go_LIVE_Manager()
    End Sub


#End Region

#Region "API IMPRESSION"



    Sub Imprimer_BT_Scenario_1(Sbt)
        Try
            Process_Impression_BT(Sbt)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Impression_BT_ServerClick(sender As Object, e As EventArgs)
        Try
            Dim Sbt = Lblbt.InnerText.Trim
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(Sbt, "-")
            If Ires <> 0 Then
                Slettre = Mid(Sbt, Ires + 1)
                Sbt = Mid(Sbt, 1, Ires - 1)
            End If
            Process_Impression_BT(Sbt)
        Catch ex As Exception

        End Try
    End Sub
    Sub Process_Impression_BT(Sbt)
        slienemploye = slienip.Trim & "GetretourneimpressionBTpdf/" & Slienpath & Sbt
        Try
            Dim Resultat As String
            Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
            client.KeepAlive = True
            Using WebResponse As HttpWebResponse = client.GetResponse()
                Dim responseStream As Stream = WebResponse.GetResponseStream()
                Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                Dim Sapi As String = getreader.ReadToEnd()
                Resultat = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                responseStream.Close()
                responseStream.Dispose()
                getreader.Close()
                getreader.Dispose()
            End Using
            Dim Valeur As String = Sbt.Trim & ".pdf"
            'Session("MaValeur") = Valeur
            '  Response.Redirect("Webaffichepdf.aspx?valeur=" & Server.UrlEncode(Valeur))
            Dim surl As String = "http://localhost/ImpressionWeb/" & Valeur.Trim
            Process.Start(surl)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OuvrirNouvelOnglet", "ouvrirNouvelOnglet();", True)
            '' Spécifiez l'URL de votre API REST
            'Dim apiUrl As String = "url_de_votre_api"

            '' Utilisez HttpClient pour effectuer l'appel REST
            'Using client2 As New HttpClient()
            '    Dim response As HttpResponseMessage = client2.GetAsync(apiUrl).Result

            '    ' Vérifiez si la demande a réussi
            '    If response.IsSuccessStatusCode Then
            '        ' Lisez le contenu du fichier PDF
            '        Dim pdfContent As Byte() = response.Content.ReadAsByteArrayAsync().Result

            '        ' Sauvegardez le contenu du PDF dans un fichier temporaire
            '        Dim tempFilePath As String = Path.GetTempFileName() + ".pdf"
            '        File.WriteAllBytes(tempFilePath, pdfContent)

            '        ' Redirigez vers la nouvelle page web qui affiche le PDF
            '        response.Redirect("AfficherPDF.aspx?filePath=" + tempFilePath)
            '    Else
            '        ' Gérez les erreurs en conséquence
            '        response.Write("Erreur lors de la récupération du fichier PDF.")
            '    End If
            'End Using



        Catch ex As Exception
            Dim Sresult1 As String = ex.Message

        End Try

    End Sub
    Protected Sub Impressionweb_ServerClick(sender As Object, e As EventArgs)
        '<NavigateUrl= "C:\WebpunchAdam\Webpunch\131263.pdf" > Exemple</asp:HyperLink>    

    End Sub
    Protected Sub Impression_BT2_ServerClick(sender As Object, e As EventArgs)
        Try
            Dim Sbt = Lblbt.InnerText.Trim
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(Sbt, "-")
            If Ires <> 0 Then
                Slettre = Mid(Sbt, Ires + 1)
                Sbt = Mid(Sbt, 1, Ires - 1)
            End If


            slienemploye = slienip.Trim & "GetretourneimpressionBTpdf/" & Slienpath & Sbt
            Try
                Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                client.KeepAlive = True
                Using WebResponse As HttpWebResponse = client.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Dim Resultat As String = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)


                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()



                End Using

                Dim Valeur As String = Sbt.Trim & ".pdf"

                If 1 = 2 Then


                    Dim surl As String = "http://localhost/ImpressionWeb/" & Valeur.Trim
                    Process.Start(surl)
                Else
                    Session("MaValeur") = Valeur
                    Response.Redirect("Webaffichepdf.aspx?valeur=" & Server.UrlEncode(Valeur))
                End If
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OuvrirNouvelOnglet", "ouvrirNouvelOnglet();", True)
                '' Spécifiez l'URL de votre API REST
                'Dim apiUrl As String = "url_de_votre_api"

                '' Utilisez HttpClient pour effectuer l'appel REST
                'Using client2 As New HttpClient()
                '    Dim response As HttpResponseMessage = client2.GetAsync(apiUrl).Result

                '    ' Vérifiez si la demande a réussi
                '    If response.IsSuccessStatusCode Then
                '        ' Lisez le contenu du fichier PDF
                '        Dim pdfContent As Byte() = response.Content.ReadAsByteArrayAsync().Result

                '        ' Sauvegardez le contenu du PDF dans un fichier temporaire
                '        Dim tempFilePath As String = Path.GetTempFileName() + ".pdf"
                '        File.WriteAllBytes(tempFilePath, pdfContent)

                '        ' Redirigez vers la nouvelle page web qui affiche le PDF
                '        response.Redirect("AfficherPDF.aspx?filePath=" + tempFilePath)
                '    Else
                '        ' Gérez les erreurs en conséquence
                '        response.Write("Erreur lors de la récupération du fichier PDF.")
                '    End If
                'End Using



            Catch ex As Exception
                Dim Sresult1 As String = ex.Message

            End Try


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Impression_BT3_ServerClick(sender As Object, e As EventArgs)
        Dim sBT As String = Badge_Status_WORKING.InnerText
        MainAsync(sBT).Wait()
    End Sub


    Async Function MaincreerimprimefactureBTAsync(sBT) As Task
        Try


            slienemploye = slienip.Trim & "GetretourneimpressionFactureBTpdf/" & Slienpath & sBT.PadLeft(10) & "V" & "F"
            'slienemploye = slienemploye.Replace("DevApi/", "")

            'slienemploye = slienemploye.Replace("6080", "6084")
            ' slienemploye = slienip.Trim & "/api/Person/GetretourneimpressionCommandepdf/" & Slienpath & Sbt
            Try
                Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                client.KeepAlive = True
                Using WebResponse As HttpWebResponse = client.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Dim Resultat As String = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)


                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()



                End Using
                If 1 = 2 Then
                    '' Dim Surl As String = slienip & "impressionweb/" & Sbt.Trim & ".pdf"
                    'Dim Surl As String = "http://localhost:44398/" & "impressionweb/" & Sbt.Trim & ".pdf"
                    '' Chemin du répertoire de téléchargement de l'utilisateur actuel
                    ''Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                    'Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImpressionWeb")
                    ''Dim Surl As String = repertoireDestination & "/" & Sbt.Trim & ".pdf"
                    'Dim Suri As String = Sbt.Trim & ".pdf"
                    'Await DownloadFileAsync(Surl, Suri)
                Else


                    'Dim host As String = "http://localhost/"
                    ' Dim host As String = "http://localhost:59768/"
                    'Dim host As String = "http://192.168.10.107:6080/"
                    'Public slienip As String = "http://173.177.123.32:6080/"
                    'Public slienip As String = "http://173.177.123.32:6080/api/Person/"
                    'Public slienip As String = "http://173.177.123.32:6080/DevApi/api/Person/"
                    'Dim Suri As String = host & "impressionweb/" & Sbt.Trim & ".pdf"
                    Dim Suri As String = slienip & "impressionweb/" & sBT.Trim & ".pdf"
                    ' Chemin du répertoire de téléchargement de l'utilisateur actuel
                    Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                    'Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Downloads")
                    Dim Surl As String = repertoireDestination & "\" & sBT.Trim & ".pdf"
                    'Dim Surl As String = "F:\Sei_inventaire\impressionweb\" & Sbt.Trim & ".pdf"
                    downloadFichier(Suri, Surl, "", "")
                    Process.Start(Surl)
                End If

            Catch ex As Exception
                Dim Sresult1 As String = ex.Message

            End Try


        Catch ex As Exception

        End Try
        ' Appeler la sous-procédure asynchrone en utilisant Await

        Console.WriteLine("Fin de l'exécution.")
    End Function


    Async Function MainAsync(sBT) As Task
        Try

            Dim slienemploye As String = ""
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(sBT, "-")
            If Ires <> 0 Then
                Slettre = Mid(sBT, Ires + 1)
                sBT = Mid(sBT, 1, Ires - 1)
            End If

            'slienemploye = slienip.Trim & "GetretourneimpressionBTpdf/" & Slienpath & sBT
            'slienemploye = slienemploye.Replace("DevApi/", "")

            'Dim sBT As String = "43075"
            'Dim slienemploye As String = ""
            'Dim Slienpath As String = "FBDMBDM"
            ''Dim Slienpath As String = "SWEBWEB"
            ''Dim slienip As String = "http://localhost:59768/"
            'Dim slienip As String = "http://seibureau.ddns.net:6080/"
            'Dim Slettre As String = ""
            'Dim Ires As Integer = InStr(sBT, "-")
            'If Ires <> 0 Then
            '    Slettre = Mid(sBT, Ires + 1)
            '    sBT = Mid(sBT, 1, Ires - 1)
            'End If
            'slienemploye = "HTTP://cobrex.dyndns.info:6080" & "/Devapi/api/Person/GetretourneimpressionBTpdf/" & Slienpath & sBT

            'slienemploye = slienip.Trim & "api/Person/GetretourneimpressionBTpdf/" & Slienpath & sBT
            slienemploye = slienip.Replace("DevApi/api/Person/", "") & "api/Person/GetretourneimpressionBTpdf/" & Slienpath & sBT

            '"http://seibureau.ddns.net:6080/api/Person/GetretourneimpressionBTpdf/FBDMBDM43075"
            '"http://cobrex.dyndns.info:6080/api/Person/GetretourneimpressionBTpdf/SWEBWEB55512"

            Try
                Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                client.KeepAlive = True
                Using WebResponse As HttpWebResponse = client.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Dim Resultat As String = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()
                End Using


                Dim Suri As String = slienip.Replace("DevApi/api/Person/", "") & "impressionweb/" & sBT.Trim & ".pdf"
                ' Chemin du répertoire de téléchargement de l'utilisateur actuel
                Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                'Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Downloads")
                Dim Surl As String = repertoireDestination & "\" & sBT.Trim & ".pdf"
                'Dim Surl As String = "F:\Sei_inventaire\impressionweb\" & Sbt.Trim & ".pdf"
                downloadFichier(Suri, Surl, "", "")
                Process.Start(Surl)

            Catch ex As Exception
                Dim Sresult1 As String = ex.Message

            End Try


        Catch ex As Exception

        End Try
        ' Appeler la sous-procédure asynchrone en utilisant Await

        Console.WriteLine("Fin de l'exécution.")
    End Function
    Protected Sub Sms_M_ServerClick(sender As Object, e As EventArgs)
        SW_SMS("BONTRAVAIL")
    End Sub
    Private Sub downloadFichier(ByVal strUrlFichier As String,
                                ByVal strCheminDestinationFichier As String,
                                ByVal identifiant As String,
                                ByVal motDePasse As String)
        ' strUrlFichier : Uri du fichier sur le serveur FTP
        ' strCheminDestinationFichier : Uri du fichier sur le disque dur
        ' identifiant : login du compte FTP utilisé
        ' motDePasse : mot de passe du compte FTP utilisé
        ' Instanciation de deux Uri qui vont contenir les chemins source et destination
        Dim monUriFichier As New System.Uri(strUrlFichier)
        Dim monUriDestinationFichier As New System.Uri(strCheminDestinationFichier)
        ' Vérification de la validité de l'Uri du fichier sur le serveur FTP
        ' If Not (monUriFichier.Scheme = Uri.UriSchemeFtp) Then '
        If Not (monUriFichier.Scheme = Uri.UriSchemeHttp) Then
            Dim s As String = "L'Uri du fichier sur le serveur FTP n'est pas valide"
            ' Si Uri non valide, arrêt du téléchargement
            Exit Sub
        End If
        ' Vérification de la validité de l'Uri de l'emplacement du fichier de destination

        If Not (monUriDestinationFichier.Scheme = Uri.UriSchemeFile) Then
            Dim s As String = "Le chemin de destination n'est pas valide !"
            ' Si Uri non valide, arrêt du téléchargement
            Exit Sub
        End If
        ' Création des 2 flux et du reader nécessaire pour la récupération du fichier
        Dim monResponseStream As Stream = Nothing
        Dim monFileStream As FileStream = Nothing
        Dim monReader As StreamReader = Nothing
        Try
            ' Requête demandant le fichier se trouvant sur le serveur FTP
            ' Dim downloadRequest As FtpWebRequest = CType(WebRequest.Create(monUriFichier), FtpWebRequest)
            Dim downloadRequest As HttpWebRequest = CType(WebRequest.Create(monUriFichier), HttpWebRequest)
            ' Vérification de la présence des identifiants d'un compte, si aucun alors
            ' la connexion se fait en mode anonyme
            If Not identifiant.Length = 0 Then
                Dim monCompteFtp As New NetworkCredential(identifiant, motDePasse)
                downloadRequest.Credentials = monCompteFtp
            End If
            ' Flux de données issu du fichier sur le serveur FTP
            'Dim downloadResponse As FtpWebResponse = CType(downloadRequest.GetResponse(), FtpWebResponse)
            Dim downloadResponse As HttpWebResponse = CType(downloadRequest.GetResponse(), HttpWebResponse)
            monResponseStream = downloadResponse.GetResponseStream()
            ' Chemin de destination du fichier sur le disque dur

            Dim nomFichier As String = monUriDestinationFichier.LocalPath.ToString
            '  nomFichier = "C:/Users/marcb/Downloads"
            ' Création du fichier de destination sur le disque dur
            monFileStream = File.Create(nomFichier)
            ' Tableau d'octets qui va contenir les données issues du flux
            Dim monBuffer(1024) As Byte
            Dim octetsLus As Integer
            ' Lecture du buffer, puis écriture des données dans le fichier
            'Setat = "3"
            'Me.BackgroundWorker8.ReportProgress(1)

            While True
                ' Lecture du flux

                octetsLus = monResponseStream.Read(monBuffer, 0, monBuffer.Length)
                If octetsLus = 0 Then
                    Exit While
                End If
                ' Ecriture dans le fichier
                monFileStream.Write(monBuffer, 0, octetsLus)

            End While
            'Setat = "3"
            'Me.BackgroundWorker8.ReportProgress(1)
            'MessageBox.Show("Téléchargement effectué.")
            ' Gestion des exceptions
            If monReader IsNot Nothing Then
                monReader.Close()
            ElseIf monResponseStream IsNot Nothing Then
                monResponseStream.Close()
            End If
            ' Fermeture du flux et du fichier
            If monFileStream IsNot Nothing Then
                monFileStream.Close()
            End If

        Catch ex As UriFormatException
            'MessageBox.Show(ex.Message)
            Dim s As String = ex.Message + vbCrLf + "Mauvaise défénition du lien"
        Catch ex As WebException
            'MessageBox.Show(ex.Message)
            Dim s As String = ex.Message + vbCrLf + "Erreur de transfert1"
        Catch ex As IOException
            'MessageBox.Show(ex.Message)
            Dim s As String = ex.Message + vbCrLf + "Erreur de transfert2"
        Finally
            ' Fermeture du reader et des deux streams si nécessaire
            If monReader IsNot Nothing Then
                monReader.Close()
            ElseIf monResponseStream IsNot Nothing Then
                monResponseStream.Close()
            End If
            ' Fermeture du flux et du fichier
            If monFileStream IsNot Nothing Then
                monFileStream.Close()
            End If
        End Try
    End Sub

    Function FileAsync_Courriel() As String
        Dim AttachmentPath As String = Nothing
        Try
            Dim Sbt = Lblbt.InnerText.Trim
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(Sbt, "-")
            If Ires <> 0 Then
                Slettre = Mid(Sbt, Ires + 1)
                Sbt = Mid(Sbt, 1, Ires - 1)
            End If

            'Dim Suri As String = host & "impressionweb/" & Sbt.Trim & ".pdf"
            Dim Suri As String = slienip.Replace("DevApi/api/Person/", "") & "impressionweb/" & Sbt.Trim & ".pdf"
            ' Chemin du répertoire de téléchargement de l'utilisateur actuel
            Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            'Suri = Suri.Replace("6080/DevApi/api/Person/impressionweb/", "6084/")
            Dim Surl As String = repertoireDestination & "\" & Sbt.Trim & ".pdf"
            AttachmentPath = Suri
            'Dim Surl As String = "F:\Sei_inventaire\impressionweb\" & Sbt.Trim & ".pdf"
            downloadFichier(Suri, Surl, "", "")
        Catch ex As Exception

        End Try
        'Process.Start(Surl)
        Return AttachmentPath
    End Function
    Function FileAsync_Courriel_External_Link() As String
        Dim AttachmentPath As String = Nothing
        Try
            Dim Sbt = Lblbt.InnerText.Trim
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(Sbt, "-")
            If Ires <> 0 Then
                Slettre = Mid(Sbt, Ires + 1)
                Sbt = Mid(Sbt, 1, Ires - 1)
            End If

            'Dim Suri As String = host & "impressionweb/" & Sbt.Trim & ".pdf"
            Dim Suri As String = slienip.Replace("DevApi/api/Person/", "") & "impressionweb/" & Sbt.Trim & ".pdf"
            ' Chemin du répertoire de téléchargement de l'utilisateur actuel
            Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            'Suri = Suri.Replace("6080/DevApi/api/Person/impressionweb/", "6084/")
            AttachmentPath = Suri
            Dim Surl As String = repertoireDestination & "\" & Sbt.Trim & ".pdf"
            'Dim Surl As String = "F:\Sei_inventaire\impressionweb\" & Sbt.Trim & ".pdf"
            downloadFichier(Suri, Surl, "", "")
        Catch ex As Exception

        End Try
        'Process.Start(Surl)
        Return AttachmentPath
    End Function
    Protected Sub btn_alerte_sms_ServerClick(sender As Object, e As EventArgs)
        SW_SMS("ALERTE")
    End Sub
    Protected Async Sub btn_send_email_alert_ServerClick(sender As Object, e As EventArgs)
        Await SW_EMAIL("ALERTE")
    End Sub
    Protected Async Sub Courriel_ServerClick(sender As Object, e As EventArgs)
        Await SW_EMAIL("BONTRAVAIL")
    End Sub
    Async Function SW_EMAIL(scenario_) As Task
        'MainAsync().Wait()
        Dim AttachmentPath As String = FileAsync_Courriel()
        Try
            Dim txtSubject As String
            Select Case True
                Case scenario_ = "ALERTE"
                    txtSubject = "Rappellez nous à propos de votre Bon de travail : " & Lblbt.InnerText
                Case scenario_ = "BONTRAVAIL"
                    txtSubject = "Voici votre Bon de travail : " & Lblbt.InnerText
            End Select
            Dim Header_Email_FR As String = "<h2> Impression PDF </h2>"
            Dim Clients_email As String = Fiche_Lblemail.InnerText
            Dim Clients_comp As String = "Mr.Menard"
            Dim Clients_email_cc As String = "info@groupesei.com"
            Dim Clients_comp_cc As String = "Groupe SEI"
            Dim txtMessage_Start As String = "<html> <body> "
            Dim txtMessage_End As String = "</body> </html> "
            Dim txtMessage As String = " Lien : " & AttachmentPath
            If Clients_email = Nothing Then
                Clients_email = "info@groupesei.com"
            End If
            txtMessage =
            txtMessage_Start &
            Header_Email_FR & txtMessage &
              txtMessage_End

            Await SendEmailAsync("CC", Clients_email, txtSubject, txtMessage, Clients_comp, Clients_email_cc, Clients_comp_cc, AttachmentPath)


        Catch ex As Exception
            Dim Sresult1 As String = ex.Message

        End Try
    End Function
    Sub SW_SMS(scenario_)
        Dim AttachmentPath As String = FileAsync_Courriel_External_Link()
        Dim Number_ As String
        Try
            Dim Code_ As String = Nothing
            Number_ = Fiche_Lbltelct.InnerText
            If Number_ = Nothing Then
                Number_ = "5149196857"
            End If

            Number_ = Number_.Replace("-", "")
            Select Case True
                Case scenario_ = "ALERTE"
                    Code_ = "Rappellez nous à propos de votre Bon de travail : " & Lblbt.InnerText & " Lien : " & AttachmentPath
                Case scenario_ = "BONTRAVAIL"
                    Code_ = "Voici votre Bon de travail : " & Lblbt.InnerText & " Lien : " & AttachmentPath
            End Select

            Mms_Send(Number_, Code_)

        Catch ex As Exception
            Dim Sresult1 As String = ex.Message

        End Try
    End Sub
#End Region

#Region "SortirdeJob"
    Sub Load_Scenario_SortirdeJob(Scenario_, BondeTravail, LettreTravail, semp, ssortie, SCompagnie)
        Try
            If ssortie <> "" Then
                LblmessagepucnhBT.InnerText = "Vous devez puncher l'employé"
                Exit Sub
            End If
            If BondeTravail <> "" Then
                LblmessagepucnhBT.InnerText = ""
                Dim Scommantairevisible As String = Comment_Visible.InnerText.Trim
                Dim ival As Integer = InStr(Scommantairevisible, vbCrLf)
                If ival <> 0 Then Scommantairevisible = Replace(Comment_Visible.InnerText.Trim, vbCrLf, " ")
                Dim SCommentaireinv As String = Comment_NonVisible.InnerText.Trim
                ival = InStr(SCommentaireinv, vbCrLf)
                If ival <> 0 Then SCommentaireinv = Replace(Comment_NonVisible.InnerText.Trim, vbCrLf, " ")

                Dim Setat As String = ddl_Panel_Entrerdestemps_setat.Text
                Dim scode As String = BondeTravail & "-" & LettreTravail
                Dim stg_ As String = punch(semp, scode, Scommantairevisible, SCommentaireinv, Setat)
                Select Case stg_
                    Case "Vous avez du temps de travail pas fermer"
                        Panel_Entrerdestemps.Visible = True

                        Offline_Panel.Visible = False

                        Panel_Parametre_Online_SubMenu.Visible = True
                        Panel_Parametre_Online_Menu.Visible = True
                        btn_Panel_Entrerdestemps.Visible = True
                        btn_Panel_Entrerdespieces.Visible = True
                        btn_Panel_Entrerdestravails.Visible = True

                        Panel_Entrerdespieces.Visible = True
                        Panel_Entrerdestravails.Visible = True
                        Panel_Entrerdestravails.Visible = True

                        Lbletatemploye.Text = stg_
                        Exit Sub
                    Case Else
                        Select Case Scenario_
                            Case "SortirJob_IDBondetravail_Q2_Continue_ServerClick"
                                afficheinfo_Bontravail_emp_Scenario_2(semp, SCompagnie)
                            Case "SortirJob_IDBondetravail_Q2_Close_ServerClick", "btn_Panel_PunchInOut_ServerClick"
                                Where_To_Update("employepunch", "employe", semp, "Fin de journee", "bondetravail", DefaultDataBase)
                            Case "SortirJob_IDBondetravail_Q2_Break_ServerClick"
                                Where_To_Update("employepunch", "employe", semp, "Pause", "bondetravail", DefaultDataBase)
                        End Select
                        Select Case Scenario_
                            Case "SortirJob_IDBondetravail_Q2_Close_ServerClick", "SortirJob_IDBondetravail_Q2_Break_ServerClick", "btn_Panel_PunchInOut_ServerClick"
                                PunchINOUT()
                        End Select
                End Select

            Else
                Select Case Scenario_
                    Case "SortirJob_IDBondetravail_Q2_Close_ServerClick", "SortirJob_IDBondetravail_Q2_Break_ServerClick", "btn_Panel_PunchInOut_ServerClick"
                        PunchINOUT()
                End Select
                LblmessagepucnhBT.InnerText = "le No de bon de travail est vide !"
            End If
        Catch ex As Exception

        End Try
        Select Case Scenario_
            Case "SortirJob_IDBondetravail_Q2_Close_ServerClick", "SortirJob_IDBondetravail_Q2_Break_ServerClick", "btn_Panel_PunchInOut_ServerClick"
                Try
                    Response.Redirect("GroupeSEIInc.aspx", True)
                Catch ex As ThreadAbortException
                Catch ex As Exception

                End Try
        End Select
    End Sub

    Protected Sub SortirJob_IDBondetravail_Q1_YES_ServerClick(sender As Object, e As EventArgs)
        SortirJob_IDBondetravail_Q1.Visible = False
        Panel_Entrerdestemps_Encours_2.Visible = True
        Panel_Entrerdestemps_Encours_3.Visible = True
        SortirJob_IDBondetravail_Q2.Visible = True
    End Sub
#End Region

#Region "BUTTON MARC"



    Protected Sub Btn_valider_unite_ServerClick_M(sender As Object, e As EventArgs)
        Affiche_valeur_unite(TextUniteCreation.Value, "VALIDER")
    End Sub
    Protected Sub Btn_val_client_ServerClick_M(sender As Object, e As EventArgs)
        affiche_valeur_client()
    End Sub

    Protected Sub ddb_recherche_produit_filtre_inv_reset_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_produit(kt_modal_recherche_produit_tb.Text.Trim)
    End Sub

    Protected Sub tb_recherche_produit_FOURN1_reset_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_produit(kt_modal_recherche_produit_tb.Text.Trim)
    End Sub

    Protected Sub tb_recherche_produit_GROTYPFAMCAT_reset_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_produit("Tous")
    End Sub

    Protected Sub kt_modal_recherche_produit_tb_reset_ServerClick(sender As Object, e As EventArgs)
        kt_modal_recherche_produit_tb.Text = Nothing
    End Sub

    Protected Sub kt_modal_recherche_produit_btn_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_produit(kt_modal_recherche_produit_tb.Text.Trim)
    End Sub

    Protected Sub kt_modal_recherche_btn_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_client(kt_modal_recherche_tb.Text.Trim)
    End Sub




    Protected Sub btn_BT_en_cours_employe_complet2_Click(sender As Object, e As EventArgs)
        'afficheinfo()
    End Sub
    Protected Sub btn_BT_en_cours_employe_complet_Click(sender As Object, e As EventArgs)
        ' Votre logique de traitement ici
        'afficheinfo()
    End Sub
    Protected Sub Create_Job_ServerClick(sender As Object, e As EventArgs) Handles Create_Job.ServerClick
        T_Create_Job_M()
    End Sub
    'Sub SubMenu_AI(Boolean_)
    '    Select Case True
    '        Case Boolean_ = False
    '            'Offline_Panel.Visible = True
    '            Offline_Panel.Visible = False

    '            Panel_Parametre_Online_Menu.Visible = False
    '            Panel_Parametre_Online_SubMenu.Visible = False
    '            btn_Panel_Entrerdestemps.Visible = False
    '            btn_Panel_Entrerdespieces.Visible = False
    '            btn_Panel_Entrerdestravails.Visible = False

    '            Panel_Entrerdespieces.Visible = False
    '            Panel_Entrerdestravails.Visible = False

    '            Panel_Entrerdestemps_Encours_2.Visible = False
    '            Panel_Entrerdestemps_Encours_3.Visible = False

    '            Panel_Entrerdestemps_Encours_2.Visible = False
    '            Panel_Entrerdestemps_Encours_3.Visible = False

    '            Panel_details_veh_detail.Visible = False
    '            Panel_details_cli_detail.Visible = False

    '        Case Else
    '            Offline_Panel.Visible = False

    '            Panel_Parametre_Online_SubMenu.Visible = True
    '            Panel_Parametre_Online_Menu.Visible = True
    '            btn_Panel_Entrerdestemps.Visible = True
    '            btn_Panel_Entrerdespieces.Visible = True
    '            btn_Panel_Entrerdestravails.Visible = True

    '            Panel_Entrerdespieces.Visible = True
    '            Panel_Entrerdestravails.Visible = True


    '    End Select

    'End Sub

    Protected Sub Add_Parts_Bondetravail_ServerClick(sender As Object, e As EventArgs) Handles Add_Parts_Bondetravail.ServerClick
        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Clients_web As String = Nothing,
            Fidelity_id As String = Nothing,
            BondeTravail As String = Nothing,
            LettreTravail As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, emailid, securitypin, semp, SCompagnie)
        T_Add_Parts_Bondetravail(SCompagnie)
    End Sub

    Protected Sub kt_modal_recherche_unite_btn_ServerClick(sender As Object, e As EventArgs)
        afficher_Recherche_unite(kt_modal_recherche_unite_tb.Text.Trim.PadRight(40) & TextClientCreation.Value.Trim.PadRight(7))
    End Sub

#End Region

#Region "NAVIGATION PUNCH ADAM"
    Protected Sub Btn_val_client_ServerClick(sender As Object, e As EventArgs)
        affiche_valeur_client()
    End Sub
    Protected Sub Btn_creer_bon_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Creer_Bon_De_Travail(Lblnoclientct.Text, Lblunitect.Text, btn_EntreSortie_employe_id.Text.Trim, SCompagnie)
    End Sub
    Sub Creer_Bon_De_Travail(Lblnoclientct_, Lblunitect_, btn_EntreSortie_employe_id_, SCompagnie)
        Dim BondeTravail, LettreTravail As String
        Dim Scenario_ As String = "Creer_Bon_De_Travail"

        Dim FUnite As String = Lblunitect.Text.Trim
        If Lblnoclientct.Text.Trim <> "" And Lblunitect.Text.Trim <> "" Then
            Try
                slienemploye = slienip & "GetProcessCreerBTSITEWEB/" & Slienpath & Lblnoclientct.Text.Trim.PadRight(7) & FUnite.PadRight(30) & TextjobcreationBT.Value.Trim
                Try
                    Dim reponse As String
                    Dim Sapi As String = GetAPIStringAsync(slienemploye)
                    Sapi = Sapi
                    If Sapi = Nothing Then Exit Sub
                    reponse = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                    If reponse.Trim <> "" Then
                        LblNoBtcreation.Text = reponse
                        BondeTravail = reponse.Substring(reponse.Length - 6, 6) & "-A"
                        SortirJob_IDBondetravail.Value = BondeTravail
                        Panel_CreationBT_statut.Visible = True
                    End If
                Catch ex As Exception
                    Dim Sresult1 As String = ex.Message
                End Try

                'Dim myScript As String = Create_Details_Job.Value & " " & Create_Letter_Job.Value
                'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('" & myScript & "');", True)

                ' afficherBTemployeetjob("ZZ")
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
            Catch ex As Exception

            End Try
        End If
    End Sub
    Protected Sub Btn_valider_unite_ServerClick(sender As Object, e As EventArgs)

        Try
            Dim TextUniteCreation_ As String = TextUniteCreation.Value


            slienemploye = slienip & "Getretournevehiculesiteweb/" & Slienpath & TextUniteCreation_.Trim.PadRight(30) & TextUniteCreation.Value.Trim.PadRight(30)
            Try
                Dim Vehiculecreationbt As requestvehiculeSITEWEB
                Dim Sapi As String = GetAPIStringAsync(slienemploye)
                Sapi = Sapi
                If Sapi = Nothing Then Exit Sub
                Vehiculecreationbt = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of requestvehiculeSITEWEB)(Sapi)
                Panelunite.Visible = True
                Lblunitect.Text = Vehiculecreationbt.vehicule(0).unite
                Lblmarquect.Text = Vehiculecreationbt.vehicule(0).marque
                Lblmodelect.Text = Vehiculecreationbt.vehicule(0).modele
                Lblanneect.Text = Vehiculecreationbt.vehicule(0).annee
                Lblseriect.Text = Vehiculecreationbt.vehicule(0).serie
                Lblcompteurct.Text = Vehiculecreationbt.vehicule(0).nbrheure
            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try


        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "ITEMCOMMAND"


    Protected Sub Repeaterrechercheproduit_ItemCommand(source As Object, e As RepeaterCommandEventArgs)

        Dim lblItem As Label = CType(e.Item.FindControl("CAT"), Label)
        If e.CommandName = "SelectItem" Then
            Add_Parts_id.Value = e.CommandArgument.ToString()
            Load_Scenario("Repeaterrechercheproduit_ItemCommand")
            'Dim script As String = String.Empty
            '' Vérifiez la version de Bootstrap pour utiliser le script approprié
            '' Exemple pour Bootstrap 5
            'script = "var modalElement = document.getElementById('Kt_recherche_produit');" &
            '         "var modalInstance = bootstrap.Modal.getInstance(modalElement);" &
            '         "modalInstance.hide();"
            '' Pour Bootstrap 4, décommentez et utilisez le script suivant à la place
            '' script = "$('#Kt_recherche_produit').modal('hide');"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeModalScript", script, True)

            ''script = "var modalElement = document.getElementById('kt_tab_pane_seipunch');" &
            ''     "var modalInstance = bootstrap.Modal.getInstance(modalElement);" &
            ''     "modalInstance.show();"

            ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openModalScript", script, True)
        End If
    End Sub
    Protected Sub Repeaterrechercheunite_ItemCommand(source As Object, e As RepeaterCommandEventArgs)

        If e.CommandName = "SelectItem" Then
            TextUniteCreation.Value = e.CommandArgument.ToString()

            Affiche_valeur_unite(TextUniteCreation.Value, "VALIDER")
            Panelunite.Visible = True
            Dim script As String = String.Empty
            ' Vérifiez la version de Bootstrap pour utiliser le script approprié
            ' Exemple pour Bootstrap 5
            script = "var modalElement = document.getElementById('Kt_recherche_unite');" &
                     "var modalInstance = bootstrap.Modal.getInstance(modalElement);" &
                     "modalInstance.hide();"
            ' Pour Bootstrap 4, décommentez et utilisez le script suivant à la place
            ' script = "$('#Kt_recherche_produit').modal('hide');"

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeModalScript", script, True)


            script = "var modalElement = document.getElementById('kt_tab_pane_seipunch');" &
                     "var modalInstance = bootstrap.Modal.getInstance(modalElement);" &
                     "modalInstance.show();"

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openModalScript", script, True)


        End If

    End Sub
    Protected Sub myRepeater_ItemCommand_M(sender As Object, e As RepeaterCommandEventArgs)
        'If e.CommandName = "SelectItem" Then
        '    TextClientCreation.Value = e.CommandArgument.ToString()
        '    ' Vous pouvez maintenant utiliser la valeur selectedItemID pour effectuer des opérations en fonction de la sélection.
        '    affiche_valeur_client()
        '    ' Par exemple, vous pouvez afficher les détails de l'élément sélectionné.
        '    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "fermerModal", "fermerFenetreModale();", True)
        'End If
    End Sub
#End Region

#Region "SUB OPTIMISER"

    Sub punch_A(Scode, SCompagnie)

#Region "API punch Employe"
        Dim semp As String = btn_EntreSortie_employe_id.Text.Trim
        Dim Ires As Integer = InStr(semp, "(")
        If Ires <> 0 Then
            semp = Mid(semp, 1, Ires - 2)
        End If
        ' semp = "=" & semp.Trim
        semp = semp.Trim.PadRight(30)

        Dim Sparametre As String = "?id=" & Slienpath & "&Semp=" & semp & "&SBT=" & Scode.ToString.PadLeft(10) & "&Scommantairevisible=" & "TEST" & "&SCommentaireinv=" & "TEST" & "&Setat=" & ""

        slienemploye = slienip & "GetProcessPunchBTemploye/resource" & Sparametre

        Try
            Dim reponse As String
            Dim Sapi As String = GetAPIStringAsync(slienemploye)
            Sapi = Sapi
            If Sapi = Nothing Then Exit Sub
            reponse = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
            Lbletatemploye.Text = reponse
            Select Case True
                Case reponse = "reussi"
                    afficheinfo_Bontravail_emp_Scenario_2(semp, SCompagnie)
                Case Else
                    afficheinfo_Bontravail_emp_Scenario_1(semp, SCompagnie)
            End Select
            afficheinfo_all_emp(semp, SCompagnie)

        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try
#End Region
    End Sub
    Sub Reset_Details_emp()
        Panel_details_veh_detail.Visible = False
        Panel_details_cli_detail.Visible = False
    End Sub
    Sub afficheinfo_all_emp(semp_, SCompagnie)
        Dim semp As String = "ZZ"

        Dim Dtaemploye As System.Data.DataTable = New DataSetpunch.EmployepunchDataTable
        Dim Dtremploye As System.Data.DataRow
        slienemploye = slienip & "Getretourneinfomultiemployepunch/" & Slienpath & semp
        Dim Employe As requestafficheemploye
        Try
            Dim Sapi As String = GetAPIStringAsync(slienemploye)
            Employe = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of requestafficheemploye)(Sapi)
            If Employe.ToString = Nothing Then Exit Sub
            Dtaemploye.Rows.Clear()

            For i As Integer = 0 To Employe.employe.Count - 1
                Dtremploye = Dtaemploye.NewRow()
                Try
                    Dtremploye("employe") = Employe.employe(i).employe
                    Dtremploye("nom") = Employe.employe(i).nom
                    'Dtremploye("nom") = Employe.employe(i).nom & "/" & stg_to_stg(Employe.employe(i).nomclient.ToString)
                    Dtremploye("bondetravail") = Employe.employe(i).bondetravail
                    Dtremploye("dateent") = Employe.employe(i).dateent
                    Dtremploye("heureent") = Employe.employe(i).heureent
                    Dtremploye("dateentbt") = Employe.employe(i).dateentwo
                    Dtremploye("heureentbt") = Employe.employe(i).heureentwo
                    Dtremploye("datesortie") = Employe.employe(i).datesortie
                    Dtremploye("heuresortie") = Employe.employe(i).heuresortie
                    Dtremploye("tempstrav") = Employe.employe(i).tempstrav
                    Dtremploye("tempschar") = Employe.employe(i).tempschar
                    Dtremploye("pourcentage") = Employe.employe(i).pourcentage
                    If semp_ = Employe.employe(i).employe Then
                        EmployepunchDataTable_tempstrav.InnerText = Employe.employe(i).tempstrav
                        EmployepunchDataTable_tempschar.InnerText = Employe.employe(i).tempschar
                        EmployepunchDataTable_pourcentage.InnerText = Employe.employe(i).pourcentage
                        EmployepunchDataTable_tempstrav_Agenda.InnerText = Employe.employe(i).tempstrav
                        EmployepunchDataTable_tempschar_Agenda.InnerText = Employe.employe(i).tempschar
                        EmployepunchDataTable_pourcentage_Agenda.InnerText = Employe.employe(i).pourcentage
                        EmployepunchDataTable_taches.InnerText = Process_Count_Mysql("", "bondetravailjobs", " WHERE employe LIKE '%" & Employe.employe(i).employe & "%' AND SCompagnie = '" & SCompagnie & "' ")
                    End If
                    Dtremploye("vendeur") = Employe.employe(i).vendeur
                    Dtremploye("commis") = Employe.employe(i).commis
                    Dtremploye("etat") = Employe.employe(i).etat
                    Dtaemploye.Rows.Add(Dtremploye)

                Catch ex As Exception

                End Try
                Try

                    Dim success As Boolean = InsertOrUpdateEmployeePunch(SCompagnie & Employe.employe(i).employe,
                                                                         Employe.employe(i).employe,
                                                                         Employe.employe(i).nom,
                                                                         Employe.employe(i).bondetravail,
                                                                         Employe.employe(i).dateent,
                                                                         Employe.employe(i).heureent,
                                                                         Employe.employe(i).datesortie,
                                                                         Employe.employe(i).heuresortie,
                                                                         Employe.employe(i).tempstrav,
                                                                         Employe.employe(i).pourcentage,
                                                                         Employe.employe(i).vendeur,
                                                                         Employe.employe(i).commis, SCompagnie)

                    If success Then
                        Console.WriteLine("Punch recorded/updated successfully!")
                    Else
                        Console.WriteLine("Punch recording/updating failed.")
                    End If
                Catch ex As Exception

                End Try

            Next
            Repeater_Horaire_Offline.DataSource = Dtaemploye
            Repeater_Horaire_Offline.DataBind()
            Repeater_Horaire.DataSource = Dtaemploye
            Repeater_Horaire.DataBind()
        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try
        'afficherBTemployeetjob(semp)


    End Sub
    Protected Sub Load_Getretournerequestemploye(punchID)
        ' Replace with the actual ID you want to retrieve
        Dim punchData As DataTable = GetEmployeePunchById(punchID)

        Try
            If punchData IsNot Nothing Then
                Select Case True
                    Case punchID = Nothing
                        For Each row As DataRow In punchData.Rows
                            btn_EntreSortie_employe_id.Items.Add(row("employe") & " (" & row("nom") & ")")
                        Next
                    Case Else
                        For Each row As DataRow In punchData.Rows

                            btn_EntreSortie_employe_id.Items.Add(row("employe") & " (" & row("nom") & ")")
                            btn_EntreSortie_employe_id.Text = row("employe") & " (" & row("nom") & ")"

                        Next
                End Select
            Else
                Console.WriteLine("Error retrieving employee punch.")
            End If
        Catch ex As Exception

        End Try

        'Try

        '    btn_EntreSortie_employe_id.SelectedIndex = 2
        '    'btn_EntreSortie_employe_id.Text = ""
        '    Lbletatemploye.Text = ""
        '    'afficheinfo()
        'Catch ex As Exception

        'End Try
    End Sub
    Function stg_to_stg(stg_) As String
        If stg_ = Nothing Then Exit Function
        Try
            stg_ = stg_.Trim.ToString
        Catch ex As Exception
            stg_ = Nothing
        End Try
        Return stg_
    End Function
    Function Verify_Before_Call_API(Lblbt_) As Boolean
        Dim Verify_Before_Call_API_boolean As Boolean = True

        Try
            Select Case True
                Case Lblbt_ = Nothing
                    Verify_Before_Call_API_boolean = False
                    Verify_Before_Call_API_Cancel()

                Case Lblbt_.Trim <> "-"
                    Verify_Before_Call_API_boolean = False
                    Verify_Before_Call_API_Cancel()

                Case Verify_Before_Call_API_boolean = True
                    kt_tab_pane_seipunch_alert.Visible = False

            End Select
        Catch ex As Exception
            Verify_Before_Call_API_boolean = False
            Verify_Before_Call_API_Cancel()
        End Try

        Return Verify_Before_Call_API_boolean
    End Function
    Sub Verify_Before_Call_API_Cancel()
        kt_tab_pane_seipunch_alert_lbl.InnerText = "Vous devez dépuncher votre bon de travail avant de dépuncher l'employé"
        kt_tab_pane_seipunch_alert.Visible = True
    End Sub
#End Region

#Region "afficheinfo"

    Protected Sub btn_Panel_PreFacture_Print_ServerClick(sender As Object, e As EventArgs)
        Try
            Dim sBT As String = "43076"
            Dim slienemploye As String = ""
            Dim Slienpath As String = "FBDMBDM"
            Dim slienip As String = "http://localhost:59768/"
            Dim Slettre As String = ""
            Dim Ires As Integer = InStr(sBT, "-")
            If Ires <> 0 Then
                Slettre = Mid(sBT, Ires + 1)
                sBT = Mid(sBT, 1, Ires - 1)
            End If
            '  Sbt = "197738"

            slienemploye = slienip.Trim & "/api/Person/GetretourneCreationPreFactureBTpdf/" & Slienpath & sBT.PadLeft(10) & "V" & "P"
            ' slienemploye = slienip.Trim & "/api/Person/GetretourneimpressionCommandepdf/" & Slienpath & Sbt
            Try
                Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                client.KeepAlive = True
                Using WebResponse As HttpWebResponse = client.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Dim Resultat As String = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()
                End Using
                slienemploye = slienip.Trim & "/api/Person/GetretourneimpressionPrerFactureBTpdf/" & Slienpath & sBT.PadLeft(10) & "V" & "P"
                ' slienemploye = slienip.Trim & "/api/Person/GetretourneimpressionCommandepdf/" & Slienpath & Sbt
                Try
                    Dim client2 As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                    client2.KeepAlive = True
                    Using WebResponse As HttpWebResponse = client2.GetResponse()
                        Dim responseStream As Stream = WebResponse.GetResponseStream()
                        Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                        Dim Sapi As String = getreader.ReadToEnd()
                        Dim Resultat As String = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                        responseStream.Close()
                        responseStream.Dispose()
                        getreader.Close()
                        getreader.Dispose()
                    End Using
                    'Dim host As String = "http://localhost/"
                    ' Dim host As String = "http://localhost:59768/"
                    'Dim host As String = "http://192.168.10.107:6080/"
                    'Public slienip As String = "http://173.177.123.32:6080/"
                    'Public slienip As String = "http://173.177.123.32:6080/api/Person/"
                    'Public slienip As String = "http://173.177.123.32:6080/DevApi/api/Person/"
                    Dim Suri As String = slienip & "impressionweb/" & sBT.Trim & ".pdf"
                    Dim repertoireDestination As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                    Dim Surl As String = repertoireDestination & "\" & sBT.Trim & ".pdf"
                    downloadFichier(Suri, Surl, "", "")
                    Process.Start(Surl)


                Catch ex As Exception
                    Dim Sresult1 As String = ex.Message

                End Try



            Catch ex As Exception
                Dim Sresult1 As String = ex.Message

            End Try


        Catch ex As Exception

        End Try
        ' Appeler la sous-procédure asynchrone en utilisant Await

        Console.WriteLine("Fin de l'exécution.")
    End Sub

    Protected Sub btn_Creer_Job_Step_Next_ServerClick(sender As Object, e As EventArgs)
        Dim step_ As String = Creer_Job_Step_id.InnerText
        Dim effect_ As String = "NEXT"
        Manage_Creer_Job_effect_(step_, effect_)
    End Sub

    Protected Sub btn_Creer_Job_Step_Back_ServerClick(sender As Object, e As EventArgs)
        Dim step_ As String = Creer_Job_Step_id.InnerText
        Dim effect_ As String = "BACK"
        Manage_Creer_Job_effect_(step_, effect_)
    End Sub
    Sub Manage_Creer_Job_effect_(step_, effect_)
        Panel_Entrerdestravails_alert.Visible = False
        Select Case step_
            Case "Etape 1"
                Select Case True
                    Case ddl_Panel_Entrerdestravails_Step_0.Text = "Choisir un Bon de travail"
                        Panel_CreationJOBS_alert.Visible = True
                        Panel_CreationJOBS_alert_message.InnerText = "Vous devez choisir un Bon de travail !"
                        Exit Sub
                    Case effect_ = "BACK"
                        step_ = "Etape 1"
                    Case effect_ = "NEXT"
                        step_ = "Etape 2"
                End Select
            Case "Etape 2"
                Select Case True
                    Case ddl_Panel_Entrerdestravails_Step_1.Text = "Choisir une lettre"
                        Panel_CreationJOBS_alert.Visible = True
                        Panel_CreationJOBS_alert_message.InnerText = "Vous devez choisir une lettre !"
                        Exit Sub
                    Case effect_ = "BACK"
                        step_ = "Etape 1"
                    Case effect_ = "NEXT"
                        step_ = "Etape 3"
                End Select
            Case "Etape 3"
                Select Case True
                    Case ddl_Panel_Entrerdestravails_Step_2.Text = "Choisir type"
                        Panel_CreationJOBS_alert.Visible = True
                        Panel_CreationJOBS_alert_message.InnerText = "Vous devez choisir un type !"
                        Exit Sub
                    Case effect_ = "BACK"
                        step_ = "Etape 2"
                    Case effect_ = "NEXT"
                        step_ = "Etape 4"
                End Select
            Case "Etape 4"
                Select Case True
                    Case ddl_Panel_Entrerdestravails_Step_3.Text = "Choisir Technicien"
                        Panel_CreationJOBS_alert.Visible = True
                        Panel_CreationJOBS_alert_message.InnerText = "Vous devez choisir un technicien !"
                        Exit Sub
                    Case effect_ = "BACK"
                        step_ = "Etape 3"
                    Case effect_ = "NEXT"
                        step_ = "Etape 5"
                End Select
            Case "Etape 5"
                Select Case True
                    Case ddl_Panel_Entrerdestravails_Step_4.Text = "Choisir Etat"
                        Panel_CreationJOBS_alert.Visible = True
                        Panel_CreationJOBS_alert_message.InnerText = "Vous devez choisir un etat !"
                        Exit Sub
                    Case effect_ = "BACK"
                        step_ = "Etape 4"
                    Case effect_ = "NEXT"
                        step_ = "Etape 6"
                End Select
            Case "Etape 6"
                Select Case True
                    Case effect_ = "BACK"
                        step_ = "Etape 5"
                    Case effect_ = "NEXT"
                        step_ = "Etape Finale"
                End Select
        End Select
        Creer_Job_Step_id.InnerText = step_
        Creer_Job_Step_id_2.InnerText = step_
        Select Case True
            Case step_ = "Etape 1"
                Creer_Job_Step_id_1.InnerText = "Etape 1"
            Case step_ = "Etape 2"
                Creer_Job_Step_id_1.InnerText = "Etape 2"
            Case step_ = "Etape 3"
                Creer_Job_Step_id_1.InnerText = "Etape 3"
            Case step_ = "Etape 4"
                Creer_Job_Step_id_1.InnerText = "Etape 4"
            Case step_ = "Etape 5"
                Creer_Job_Step_id_1.InnerText = "Etape 5"
            Case step_ = "Etape 6"
                Creer_Job_Step_id_1.InnerText = "Etape 6"
            Case step_ = "Etape Finale"
                Creer_Job_Step_id_1.InnerText = "Etape Finale"
        End Select
        Manage_Creer_Job_step_(step_)
    End Sub
    Sub Manage_Creer_Job_step_(step_)
        ddl_Panel_Entrerdestravails_Step_0.Visible = False
        ddl_Panel_Entrerdestravails_Step_1.Visible = False
        ddl_Panel_Entrerdestravails_Step_2.Visible = False
        ddl_Panel_Entrerdestravails_Step_3.Visible = False
        ddl_Panel_Entrerdestravails_Step_4.Visible = False
        div_Panel_Entrerdestravails_Step_5.Visible = False
        Select Case step_
            Case "Etape 1"
                ddl_Panel_Entrerdestravails_Step_0.Visible = True
            Case "Etape 2"
                ddl_Panel_Entrerdestravails_Step_1.Visible = True
            Case "Etape 3"
                ddl_Panel_Entrerdestravails_Step_2.Visible = True
            Case "Etape 4"
                ddl_Panel_Entrerdestravails_Step_3.Visible = True
            Case "Etape 5"
                ddl_Panel_Entrerdestravails_Step_4.Visible = True
            Case "Etape 6"
                div_Panel_Entrerdestravails_Step_5.Visible = True
            Case "Etape Finale"
                Panel_Entrerdestravails_success.Visible = True
        End Select
    End Sub

    Sub afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
        Dim Dtaitempiece As System.Data.DataTable = New DataSetpunch.ItempieceDataTable
        Dim Dtritempiece As System.Data.DataRow
#Region "Afficher les pieces du BT"
        Dim Ires As Integer = 0
        If BondeTravail = Nothing Then
            Exit Sub
        End If
        Dim list_ As New List(Of String)
        Dim ini_Scenario_ As String
        Try
            Dim Dict_afficheinfo_Bontravail_parts As New Dictionary(Of String, String) From {
                {"btn_Panel_Bontravail_ServerClick", "BT"},
                {"afficheinfo_Bontravail_emp_Scenario_3", "BT"},
                {"T_Create_Job_M", "EXIT"},
                {"btn_Panel_Entrerdespieces_ServerClick", "ALL"},
                {"T_Add_Parts_Bondetravail", "ALL"},
                {"Filter_Letter_Parts", "ALL"},
                {"btn_Panel_Entrerdestravails_ServerClick", "EXIT"},
                {"Filter_Letter_Tasks", "EXIT"},
                {"KeyEND", "Value3"}
            }

            ini_Scenario_ = GetValueIfKeyExists(scenario_, Dict_afficheinfo_Bontravail_parts)
            Select Case ini_Scenario_
                Case "Key not found in dictionary."
                    scenario_ = scenario_
                    afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
                    Exit Sub
                Case "EXIT"
                    Exit Sub
            End Select
        Catch ex As Exception
            scenario_ = scenario_
            Exit Sub
        End Try


        Try






            'Try

            '    Ires = InStr(BondeTravail, "-")
            '    If Ires <> 0 Then
            '        If LettreTravail = Nothing Then
            '            LettreTravail = BondeTravail.Substring(Ires, 1)
            '        End If
            '        BondeTravail = Mid(BondeTravail, 1, Ires - 1)
            '    End If
            '    slienemploye = slienip & "GetretourneBondeTravailPiece/" & Slienpath & BondeTravail

            '    Dim Btpiece As RequestBTpiece
            '    Dim Sapi As String = GetAPIStringAsync(slienemploye)
            '    Sapi = Sapi
            '    If Sapi = Nothing Then Exit Sub
            '    Btpiece = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of RequestBTpiece)(Sapi)


            '    For i As Integer = 0 To Btpiece.Fitemsbtpiece.Count - 1
            '        If Not list_.Contains(Btpiece.Fitemsbtpiece(i).FLettre) Then
            '            list_.Add(Btpiece.Fitemsbtpiece(i).FLettre)
            '        End If
            '        Dim success As Boolean = InsertOrUpdateBondeTravail(BondeTravail & "-" & Btpiece.Fitemsbtpiece(i).FLineNumber, Btpiece.Fitemsbtpiece(i).FLettre,
            '                                                            Btpiece.Fitemsbtpiece(i).FLineNumber, Btpiece.Fitemsbtpiece(i).FPartnumber,
            '                                                            Btpiece.Fitemsbtpiece(i).FDescription, Btpiece.Fitemsbtpiece(i).FLettre,
            '                                                            Btpiece.Fitemsbtpiece(i).FQte, Btpiece.Fitemsbtpiece(i).FPrice,
            '                                                            Btpiece.Fitemsbtpiece(i).FMontant, Btpiece.Fitemsbtpiece(i).Fstatus,
            '                                                            Btpiece.Fitemsbtpiece(i).Freference, Btpiece.Fitemsbtpiece(i).Fetat, FUnite, FClient, employe, SCompagnie)

            '        If success Then
            '            Console.WriteLine("Punch recorded/updated successfully!")
            '        Else
            '            Console.WriteLine("Punch recording/updating failed.")
            '        End If

            '    Next

            'Catch ex As Exception
            '    'Sresult = ex.Message
            'End Try


            Try
                Dim Get_Data_ As New DataTable
                Get_Data_.Columns.Add("FPartnumber", GetType(String))
                Get_Data_.Columns.Add("FLineNumber", GetType(String))
                Get_Data_.Columns.Add("FDescription", GetType(String))
                Get_Data_.Columns.Add("Freference", GetType(String))
                Get_Data_.Columns.Add("Fstatus", GetType(String))
                Get_Data_.Columns.Add("FLettre", GetType(String))
                Get_Data_.Columns.Add("FQte", GetType(String))

                Dim WhereLine As String


                Select Case ini_Scenario_
                    Case "ALL"
                        Select Case True
                            Case scenario_ <> "Filter_Letter_Parts"
                                WhereLine = " WHERE BondeTravail LIKE '%" & BondeTravail & "%' ORDER BY LettreTravail ASC"
                            Case scenario_ = "Filter_Letter_Parts" And LettreTravail = "ALL" And tb_Panel_Entrerdespieces_Description.Text <> Nothing Or scenario_ = "Filter_Letter_Parts" And LettreTravail = "Choisir une Lettre" And tb_Panel_Entrerdespieces_Description.Text <> Nothing
                                WhereLine = " WHERE BondeTravail LIKE '%" & BondeTravail & "%' AND FDescription LIKE '%" & tb_Panel_Entrerdespieces_Description.Text & "%' OR BondeTravail LIKE '%" & BondeTravail & "%' AND FPartnumber LIKE '%" & tb_Panel_Entrerdespieces_Description.Text & "%' ORDER BY LettreTravail ASC"
                            Case scenario_ = "Filter_Letter_Parts" And LettreTravail = "ALL" Or scenario_ = "Filter_Letter_Parts" And LettreTravail = "Choisir une Lettre"
                                WhereLine = " WHERE BondeTravail LIKE '%" & BondeTravail & "%' ORDER BY LettreTravail ASC"
                            Case scenario_ = "Filter_Letter_Parts"
                                WhereLine = " WHERE BondeTravail LIKE '%" & BondeTravail & "-" & LettreTravail & "%' ORDER BY FLineNumber ASC"
                            Case Else
                                scenario_ = scenario_
                        End Select

                    Case "BT"
                        WhereLine = " WHERE BondeTravail LIKE '%" & BondeTravail & "-" & LettreTravail & "%' ORDER BY FLineNumber ASC"
                End Select


                Dim Get_Data_2 As DataTable = GetData("afficheinfo_Bontravail_parts", "bondetravailpiece", WhereLine, 1, 10000)

                For Each row As DataRow In Get_Data_2.Rows
                    Dim newRow As DataRow = Get_Data_.NewRow()
                    Dim FPartnumber As String = row("FPartnumber")
                    Dim FLineNumber As String = row("FLineNumber")
                    Dim FDescription As String = row("FDescription")
                    Dim Freference As String = row("Freference")
                    Dim Fstatus As String = row("Fstatus")
                    Dim FLettre As String = row("FLettre")
                    Dim FQte As String = row("FQte")

                    newRow("FPartnumber") = FPartnumber
                    newRow("FLineNumber") = FLineNumber
                    newRow("FDescription") = FDescription
                    newRow("Freference") = Freference
                    newRow("Fstatus") = Fstatus
                    newRow("FLettre") = FLettre
                    newRow("FQte") = FQte
                    Get_Data_.Rows.Add(newRow)

                    If Not list_.Contains(FLettre) Then
                        list_.Add(FLettre)
                    End If

                Next

                If Get_Data_.Rows.Count > 0 Then
                    ViewState("dt") = Get_Data_
                    Select Case True
                        Case scenario_ = "btn_Panel_Entrerdespieces_ServerClick" Or scenario_ = "Filter_Letter_Parts" Or scenario_ = "T_Add_Parts_Bondetravail"
                            Rep_Liste_des_pieces.DataSource = Get_Data_
                            Rep_Liste_des_pieces.DataBind()
                        Case scenario_ = "afficheinfo_Bontravail_emp_Scenario_3" Or scenario_ = "btn_Panel_Bontravail_ServerClick"
                            Rep_Liste_des_pieces_Taches.DataSource = Get_Data_
                            Rep_Liste_des_pieces_Taches.DataBind()
                        Case scenario_ = "btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick"
                            Repeater_Metronic_allWO_Load_Parts.DataSource = Get_Data_
                            Repeater_Metronic_allWO_Load_Parts.DataBind()
                        Case Else
                            scenario_ = scenario_
                    End Select
                Else
                    Select Case True
                        Case scenario_ = "ALL"
                            Rep_Liste_des_pieces_Taches.Visible = False
                        Case scenario_ = "BT"
                            Rep_Liste_des_pieces.Visible = False
                        Case scenario_ = "btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick"
                            Repeater_Metronic_allWO_Load_Parts.Visible = False
                    End Select
                End If

            Catch ex As Exception

            End Try


            If ini_Scenario_ = "ALL" And LettreTravail = "Choisir une lettre" Or ini_Scenario_ = "ALL" And LettreTravail = "Tous les jobs" Then

                Select Case ini_Scenario_
                    Case "ALL"
                        dp_Filter_Letter_Parts.Items.Clear()
                        dp_Filter_Letter_Parts.Items.Add("Choisir une Lettre")
                        dp_Filter_Letter_Parts.Items.Add("Tous les jobs")
                End Select

                If LettreTravail.ToString.Length < 3 Then
                    If Not list_.Contains(LettreTravail) Then
                        list_.Add(LettreTravail)
                    End If
                End If
                For Each item As String In list_
                    dp_Filter_Letter_Parts.Items.Add(New ListItem(item))
                Next
                Select Case True
                    Case ini_Scenario_ = "ALL" And scenario_ <> "Filter_Letter_Parts"
                        dp_Filter_Letter_Parts.Text = "Tous les jobs"
                    Case Else
                        dp_Filter_Letter_Parts.Text = LettreTravail
                End Select
            End If


        Catch ex As Exception

        End Try
#End Region
    End Sub

#Region "afficheinfo_Bontravail_emp"


    Sub affiche_Update_Info_Scenario(scenario_, BondeTravail, LettreTravail, semp)
        Dim stg_ As String
        Select Case scenario_
            Case "afficheinfo_Bontravail_emp_Scenario_1"
                stg_ = "Entrée dans la compagnie"
            Case "afficheinfo_Bontravail_emp_Scenario_2", "afficheinfo_Bontravail_emp_Scenario_3"
                stg_ = "Pause / Fin de Journée"
            Case "afficheinfo_Bontravail_emp_Scenario_3"
                stg_ = "Sortir"
        End Select

        lbl_btn_SortirdeJob.InnerText = stg_
        lbl_btn_SortirdeJob_2.InnerText = stg_
        lbl_btn_SortirdeJob_4.InnerText = stg_
        lbl_btn_SortirdeJob_3.InnerText = stg_

        affiche_ui_info(scenario_, BondeTravail, LettreTravail)
    End Sub
    Sub affiche_Update_Info(semp, SCompagnie)
        Dim dateent, BondeTravail, datesortie, LettreTravail, FUnite, scenario_ As String
        afficheinfo_Bontravail_emp(semp, BondeTravail, LettreTravail, dateent, datesortie, FUnite)

        Load_Bon_Travail_Info(BondeTravail, LettreTravail)
        ddl_Panel_Entrerdestravails_Step_0.Items.Add(BondeTravail)
        ddl_Panel_Entrerdestravails_Step_0.Text = BondeTravail
        ddl_Panel_Entrerdestravails_Step_1.Text = LettreTravail

        Dim Get_Data_2 As DataTable = GetData("", "employepunch", " WHERE SCompagnie = '" & SCompagnie & "'", 1, 10000)

        Dim Get_Data_ As New DataTable
        Get_Data_.Columns.Add("nom", GetType(String))
        For Each row As DataRow In Get_Data_2.Rows
            Dim newRow As DataRow = Get_Data_.NewRow()
            Dim employe As String = row("employe").ToString.Trim
            Dim nom As String = row("nom").ToString.Trim

            newRow("nom") = nom & " (" & employe & ")"
            Get_Data_.Rows.Add(newRow)
        Next

        Select Case True
            Case datesortie <> Nothing
                scenario_ = "afficheinfo_Bontravail_emp_Scenario_1"
                affiche_Update_Info_Scenario(scenario_, BondeTravail, LettreTravail, semp)
                afficher_BT_Jobs(scenario_, Nothing, Nothing, SCompagnie, FUnite)

            Case datesortie = Nothing And BondeTravail <> Nothing
                scenario_ = "afficheinfo_Bontravail_emp_Scenario_3"
                Dim FClient As String = Fiche_Lbl_Client_id.InnerText.Trim


                affiche_Update_Info_Scenario(scenario_, BondeTravail, LettreTravail, semp)
                afficher_BT_Jobs(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)

            Case datesortie = Nothing And BondeTravail <> Nothing
                Try
                    Response.Redirect("GroupeSEIInc.aspx", True)
                Catch ex As ThreadAbortException
                Catch ex As Exception

                End Try

            Case datesortie = Nothing And BondeTravail = Nothing
                scenario_ = "afficheinfo_Bontravail_emp_Scenario_2"
                affiche_Update_Info_Scenario(scenario_, BondeTravail, LettreTravail, semp)
                afficher_BT_Jobs(scenario_, Nothing, Nothing, SCompagnie, FUnite)
                afficheinfo_all_wo(scenario_, Nothing, "FLASH", "ALL", SCompagnie)

        End Select

    End Sub

    Sub affiche_ui_badge(Badge_Status_String)
        Badge_Status_WAIT.InnerText = Badge_Status_String
        Badge_Status_WAIT_1.InnerText = Badge_Status_String
        Badge_Status_WAIT_2.InnerText = Badge_Status_String
        Badge_Status_WAIT_3.InnerText = Badge_Status_String
        Badge_Status_WAIT_4.InnerText = Badge_Status_String
        Badge_Status_WAIT_5.InnerText = Badge_Status_String
        Badge_Status_WAIT_6.InnerText = Badge_Status_String
        Badge_Status_WORKING.InnerText = Badge_Status_String
    End Sub
    Sub affiche_ui_info(scenario_, BondeTravail, LettreTravail)
        Dim Badge_Status_String As String = Nothing
        Select Case scenario_

            Case "afficheinfo_Bontravail_emp_Scenario_1"
                'SubMenu_AI(False)
                Panel_Menu_Taches.Visible = True
                Panel_Menu_Taches_Offline_1.Visible = True
                div_Badge_Status_WAIT.Visible = True
                Panel_Menu_Taches_Online_1.Visible = False
                div_btn_Panel_Entrerdestemps_ServerClick.Visible = False

                div_affiche_bt_info_detail_commis.Visible = False
                div_affiche_bt_info_detail_vendeur.Visible = False
                div_affiche_bt_info_detail_client.Visible = False

                Badge_Status_String = "Employé inactif"
                Panel_TreeViewBtemploye_M.Visible = False
                Panel_Parametre_Online_SubMenu.Visible = False
                btn_Panel_details_emp_Client.Visible = False
                btn_Panel_details_emp_Vehicule.Visible = False
                Offline_Panel.Visible = False
                Panel_Entrerdestemps.Visible = False
                Panel_Parametre_Online_Menu.Visible = False
                wo_actuel.Visible = True

            Case "afficheinfo_Bontravail_emp_Scenario_2"
                'SubMenu_AI(False)
                Panel_Menu_Taches.Visible = True
                Panel_Menu_Taches_Online_1.Visible = True
                Offline_Panel.Visible = False
                Panel_Menu_Taches_Offline_1.Visible = False

                div_affiche_bt_info_detail_commis.Visible = False
                div_affiche_bt_info_detail_vendeur.Visible = False
                div_affiche_bt_info_detail_client.Visible = False

                Panel_Parametre_Online_SubMenu.Visible = False
                Panel_Parametre_Online_Menu.Visible = False
                btn_Panel_details_emp_Client.Visible = False
                btn_Panel_details_emp_Vehicule.Visible = False
                'Panel_Entrerdestemps.Visible = True
                div_Badge_Status_WAIT.Visible = True
                Panel_TreeViewBtemploye_M.Visible = True


                btn_Panel_Entrerdestravails.Visible = False
                btn_Panel_Bontravail.Visible = False
                btn_Panel_Entrerdespieces.Visible = False

                Badge_Status_String = "En attente"
                lbl_btn_SortirdeJob.InnerText = "Pause / Fin de Journée"
                lbl_btn_SortirdeJob_2.InnerText = "Pause / Fin de Journée"

            Case "afficheinfo_Bontravail_emp_Scenario_3"
                'SubMenu_AI(True)
                div_btn_Panel_PunchInOut.Visible = False
                div_btn_Panel_Entrerdestemps_ServerClick.Visible = False

                div_btn_Panel_PunchInOut_2.Visible = True
                btn_Panel_Entrerdestemps.Visible = True
                Panel_Menu_Taches_Offline_1.Visible = False
                Panel_Menu_Taches.Visible = True
                Panel_Menu_Taches_Online_1.Visible = True
                btn_Panel_CreationBT.Visible = True
                btn_Panel_details_emp_Client.Visible = True
                Panel_Parametre_Online_SubMenu.Visible = True
                btn_Panel_details_emp_Vehicule.Visible = True

                Panel_Taches_Entrerdespieces.Visible = True
                Panel_Taches_Entrerdestravails.Visible = True

                Badge_Status_String = BondeTravail & "-" & LettreTravail
                lbl_btn_SortirdeJob.InnerText = "Sortie"
                lbl_btn_SortirdeJob_2.InnerText = "Sortie"

                Offline_Panel.Visible = False
                Panel_TreeView_kt_modal_allWO.Visible = False
                Panel_TreeViewBtemploye_M.Visible = False
        End Select


        affiche_ui_badge(Badge_Status_String)

    End Sub
    Sub afficheinfo_Bontravail_emp_Scenario_3(BondeTravail, LettreTravail, semp, SCompagnie)
        Dim FUnite As String = Nothing
        Dim Scenario_ As String = "afficheinfo_Bontravail_emp_Scenario_3"
        afficheinfo_Bontravail_emp(semp, BondeTravail, LettreTravail, Nothing, Nothing, FUnite)
        affiche_Update_Info_Scenario("afficheinfo_Bontravail_emp_Scenario_3", BondeTravail, LettreTravail, semp)

        afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
    End Sub
    Sub afficheinfo_Bontravail_emp_Scenario_2(semp, SCompagnie)
        Dim FUnite As String = Nothing
        Dim Scenario_ As String = "afficheinfo_Bontravail_emp_Scenario_2"
        afficheinfo_Bontravail_emp(semp, Nothing, Nothing, Nothing, Nothing, FUnite)
        affiche_Update_Info_Scenario("afficheinfo_Bontravail_emp_Scenario_2", Nothing, Nothing, semp)
        afficher_BT_Jobs(Scenario_, Nothing, Nothing, SCompagnie, FUnite)
    End Sub
    Sub afficheinfo_Bontravail_emp_Scenario_1(semp, SCompagnie)
        Dim FUnite As String = Nothing
        Dim Scenario_ As String = "afficheinfo_Bontravail_emp_Scenario_1"
        afficheinfo_Bontravail_emp(semp, Nothing, Nothing, Nothing, Nothing, FUnite)
        affiche_Update_Info_Scenario("afficheinfo_Bontravail_emp_Scenario_1", Nothing, Nothing, semp)
        afficher_BT_Jobs(Scenario_, Nothing, Nothing, SCompagnie, FUnite)
    End Sub
    Sub afficheinfo_Bontravail_emp(semp, ByRef BondeTravail, ByRef LettreTravail, ByRef dateent, ByRef datesortie, ByRef FUnite)
        Select Case True
            Case Else
                Dim Ires As Integer = InStr(semp, "(")
                If Ires <> 0 Then
                    semp = Mid(semp, 1, Ires - 2)
                End If
        End Select

        semp = semp.Trim.PadRight(30)
        slienemploye = slienip & "Getretourneinfoemploye/" & Slienpath & semp
        Try
            Dim Sapi As String = GetAPIStringAsync(slienemploye)
            Dim Employe As reponseafficheemploye
            Sapi = Sapi
            If Sapi = Nothing Then Exit Sub
            Employe = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of reponseafficheemploye)(Sapi)
            Dim bondetravail__ As String = Employe.bondetravail.Trim
            Dim LettreTravail__ As String = Employe.lettre.Trim
            Dim dateent_ As String = Employe.dateent
            Dim datesortie_ As String = Employe.datesortie
            Try
                Dim employe_ As String = stg_to_stg(Employe.employe)
                Dim nom_ As String = stg_to_stg(Employe.nom)
                Dim bondetravail_ As String = stg_to_stg(Employe.bondetravail)
                dateent_ = stg_to_stg(Employe.dateent)
                Dim heureent_ As String = stg_to_stg(Employe.heureent)
                datesortie_ = stg_to_stg(Employe.datesortie)
                Dim heuresortie_ As String = stg_to_stg(Employe.heuresortie)
                Dim tempstrav_ As String = stg_to_stg(Employe.tempstrav)
                Dim tempschar_ As String = stg_to_stg(Employe.tempschar)
                Dim pourcentage_ As String = stg_to_stg(Employe.pourcentage)
                Dim unite_ As String = stg_to_stg(Employe.unite)
                FUnite = stg_to_stg(Employe.unite)
                Dim marque_ As String = stg_to_stg(Employe.marque)
                Dim modele_ As String = stg_to_stg(Employe.modele)
                Dim client_ As String = stg_to_stg(Employe.client)
                Fiche_Lbl_Client_id.InnerText = client_
                Fiche_Lbl_Client_id_2.InnerText = client_
                'Find_Invoice(client_)


                Dim nomclient_ As String = stg_to_stg(Employe.nomclient)
                affiche_bt_info_detail_client.InnerText = nomclient_
                Dim vendeur_ As String = stg_to_stg(Employe.vendeur)
                affiche_bt_info_detail_Vendeur.InnerText = vendeur_

                Dim commis_ As String = stg_to_stg(Employe.commis)
                affiche_bt_info_detail_commis.InnerText = commis_

                Dim dateentwo_ As String = stg_to_stg(Employe.dateentwo)
                Dim heureentwo_ As String = stg_to_stg(Employe.heureentwo)
                Dim lettre_ As String = stg_to_stg(Employe.lettre)

            Catch ex As Exception

            End Try
            SortirJob_IDBondetravail.Value = Employe.bondetravail.Trim
            Lblclient.InnerText = Employe.client.Trim
            Comment_NonVisible.InnerText = ""
            Comment_Visible.InnerText = ""
            Lblbt.InnerText = Employe.bondetravail.Trim
            Lblunite.InnerText = Employe.unite.Trim
            dateent = Employe.dateent.Trim
            datesortie = Employe.datesortie.Trim
            LettreTravail = Employe.lettre.Trim
            BondeTravail = Employe.bondetravail.Trim
        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try

    End Sub

#End Region
#Region "afficheinfo_Bontravail_veh"


    Sub Reset_Panel_details_veh_detail()
        Panel_details_veh_detail_fiche.Visible = False
        Panel_details_veh_detail_historique_jobs.Visible = False
        Panel_details_veh_detail_historique_bt.Visible = False
    End Sub
    Protected Sub btn_Panel_details_veh_detail_historique_bt_ServerClick(sender As Object, e As EventArgs)
        afficheinfo_veh_bt()
    End Sub

    Protected Sub btn_Panel_details_veh_detail_fiche_historique_ServerClick(sender As Object, e As EventArgs)
        Reset_Panel_details_veh_detail()
        Panel_details_veh_detail_historique_bt.Visible = True
        afficheinfo_veh_bt()
    End Sub
    Protected Sub btn_Panel_details_veh_detail_historique_bt_back_ServerClick(sender As Object, e As EventArgs)
        Reset_Panel_details_veh_detail()
        Panel_details_veh_detail_fiche.Visible = True
    End Sub
    Protected Sub Repeater_Panel_details_veh_detail_historique_bt_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Select Case True
            Case e.CommandName = "PRINT"
                MaincreerimprimefactureBTAsync(e.CommandArgument).Wait()

            Case Else
                Info_Panel_details_veh_detail_historique_jobs.InnerText = e.CommandArgument
                afficheinfo_veh_Historique(e.CommandArgument)
                Reset_Panel_details_veh_detail()
                Panel_details_veh_detail_historique_jobs.Visible = True
        End Select

    End Sub

    Protected Sub btn_Panel_details_veh_detail_historique_jobs_ServerClick(sender As Object, e As EventArgs)
        afficheinfo_veh_Historique(Info_Panel_details_veh_detail_historique_jobs.InnerText)
    End Sub
    Protected Sub btn_Panel_details_veh_detail_historique_jobs_back_ServerClick(sender As Object, e As EventArgs)
        Reset_Panel_details_veh_detail()
        Panel_details_veh_detail_historique_bt.Visible = True
    End Sub
    Protected Sub Repeater_Panel_details_veh_detail_historique_jobs_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Imprimer_BT_Scenario_1(e.CommandArgument)
    End Sub

    Sub afficheinfo_veh_bt()
        Dim Clients_id_ As String = Nothing,
           semp As String = Nothing,
           Clients_web As String = Nothing,
           Fidelity_id As String = Nothing,
           Query_Langue As String = Nothing,
           SCompagnie As String = Nothing,
           emailid As String = Nothing,
           securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, emailid, securitypin, semp, SCompagnie)

        Dim tb_Search As String = tb_Panel_details_veh_detail_historique_bt.Text
        Dim FUnite As String = Lblunite_unite.InnerText
        Try
            Dim Get_Data_ As New DataTable
            Get_Data_.Columns.Add("FNoBT", GetType(String))
            Get_Data_.Columns.Add("FUnite", GetType(String))
            Get_Data_.Columns.Add("FEtat", GetType(String))
            Get_Data_.Columns.Add("FSerie", GetType(String))
            Get_Data_.Columns.Add("FNotefinal", GetType(String))
            Get_Data_.Columns.Add("FNom", GetType(String))
            Get_Data_.Columns.Add("Jobs", GetType(String))

            Dim Get_Data_2 As DataTable
            Select Case True
                Case tb_Search = Nothing
                    Get_Data_2 = GetData("afficheinfo_veh_bt", "fbt", " WHERE SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "'", 1, 10000)

                Case Else
                    Get_Data_2 = GetData("afficheinfo_veh_bt", "fbt", " WHERE FNoBT LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FNom LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FAdresse LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FCodePostal LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FVille LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FTelephone LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FUnite LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FMarque LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FModele LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FAnnee LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FSerie LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FCommis LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FVendeur LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "' OR " &
                                                         "FEtat LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FUnite = '" & FUnite & "'", 1, 10000)

            End Select


            For Each row As DataRow In Get_Data_2.Rows
                Dim newRow As DataRow = Get_Data_.NewRow()
                Dim BondeTravail As String = row("FNoBT").ToString.Trim

                Dim FMarque As String = row("FMarque").ToString.Trim
                Dim FModele As String = row("FModele").ToString.Trim
                Dim FAnnee As String = row("FAnnee").ToString.Trim
                Dim FSerie As String = row("FSerie").ToString.Trim
                Dim FNom As String = row("FNom").ToString.Trim
                Dim FNotefinal As String = row("FNotefinal").ToString.Trim

                Dim dateCreation As String = row("FDate").ToString.Trim
                Dim difference As Integer = ComparerDates(dateCreation)
                Dim Delaie As String = difference.ToString


                If FNotefinal <> Nothing Then
                    FNotefinal = FNotefinal
                End If

                Dim FDescription As String = Nothing
                Dim FEtat As String = Nothing
                Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")

                newRow("FNotefinal") = FNotefinal
                newRow("FNoBT") = BondeTravail
                newRow("FUnite") = FAnnee & " / " & FMarque & " / " & FModele
                newRow("FEtat") = row("FEtat")
                newRow("Jobs") = ProductToShow_.ToString
                newRow("FNom") = row("FNom") & GetColumnValueFromDB("fbt", "FNoBT", BondeTravail, "FEtat", SCompagnie)
                newRow("FSerie") = FSerie & " - " & FUnite
                Get_Data_.Rows.Add(newRow)
            Next

            If Get_Data_.Rows.Count > 0 Then
                ViewState("dt") = Get_Data_
                Repeater_Panel_details_veh_detail_historique_bt.DataSource = Get_Data_
                Repeater_Panel_details_veh_detail_historique_bt.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Sub afficheinfo_veh_Historique(FUnite)

        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Clients_web As String = Nothing,
            Fidelity_id As String = Nothing,
            BondeTravail As String = Nothing,
            LettreTravail As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, emailid, securitypin, semp, SCompagnie)

        Dim tb_Search As String = tb_Panel_details_veh_detail_historique_jobs.Text

        Try

            Dim Get_Data_2 As DataTable
            Select Case True
                Case tb_Search = Nothing
                    Get_Data_2 = GetData("afficheinfo_veh_Historique", "bondetravailjobs", " WHERE SCompagnie = '" & SCompagnie & "' AND BondeTravail = '" & FUnite & "' ORDER BY FLinenumber", 1, 10000)

                Case Else
                    Get_Data_2 = GetData("afficheinfo_veh_Historique", "bondetravailjobs", " WHERE FEtat LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND BondeTravail = '" & FUnite & "' OR " &
                                                   "FDescription LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND BondeTravail = '" & FUnite & "' ORDER BY FLinenumber", 1, 10000)

            End Select

            If Get_Data_2.Rows.Count > 0 Then
                ViewState("dt") = Get_Data_2
                Repeater_Panel_details_veh_detail_historique_jobs.DataSource = Get_Data_2
                Repeater_Panel_details_veh_detail_historique_jobs.DataBind()
            End If

        Catch ex As Exception

        End Try

        Try
            Dim Get_Data_2 As DataTable
            Select Case True
                Case tb_Search = Nothing
                    Get_Data_2 = GetData("afficheinfo_veh_Historique", "bondetravailpiece", " WHERE SCompagnie = '" & SCompagnie & "' AND BondeTravail LIKE '%" & FUnite & "%'", 1, 10000)

                Case Else
                    Get_Data_2 = GetData("afficheinfo_veh_Historique", "bondetravailpiece", " WHERE FPartnumber LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND BondeTravail LIKE '%" & FUnite & "%' OR " &
                                             "BondeTravail LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND BondeTravail LIKE '%" & FUnite & "%' OR " &
                                             "FDescription LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND BondeTravail LIKE '%" & FUnite & "%'", 1, 10000)

            End Select

            If Get_Data_2.Rows.Count > 0 Then
                ViewState("dt") = Get_Data_2
                Repeater_Panel_details_veh_detail_historique_jobs_parts.DataSource = Get_Data_2
                Repeater_Panel_details_veh_detail_historique_jobs_parts.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Sub afficheinfo_veh(Lblunite_, SCompagnie)
        Affiche_valeur_unite(Lblunite_, "TEMPS")
        If Lblunite_ = "Unité: " Then
            Lblunite_ = Nothing
        End If
        If Lblunite_ = Nothing Then
            Exit Sub
        End If
        Lblunite_ = Lblunite_.trim
        slienemploye = slienip & "GetretournefacturedelunitePunch/" & Slienpath & Lblunite_
        Try
            Dim BThistorique As RequestBT
            Dim Sapi As String = GetAPIStringAsync(slienemploye)
            Sapi = Sapi
            If Sapi = Nothing Then Exit Sub
            BThistorique = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of RequestBT)(Sapi)

            For i As Integer = 0 To BThistorique.FBT.Count - 1
                Dim FNotefinal As String = BThistorique.FBT(i).FNotefinal
                If FNotefinal <> Nothing Then
                    FNotefinal = FNotefinal
                End If
                Dim FClient As String = BThistorique.FClient
                Dim success As Boolean = InsertOrUpdateHeaderBondeTravail(BThistorique.FBT(i).FNoBT, FClient, BThistorique.FBT(i).FNom, BThistorique.FBT(i).FAdresse, BThistorique.FBT(i).FProvince,
                                             BThistorique.FBT(i).FCodePostal, BThistorique.FBT(i).FVille, BThistorique.FBT(i).FTelephone,
                                           BThistorique.FBT(i).FDate, BThistorique.FBT(i).FGtotal, BThistorique.FBT(i).FUnite, BThistorique.FBT(i).FMarque,
                                           BThistorique.FBT(i).FModele, BThistorique.FBT(i).FAnnee, BThistorique.FBT(i).FSerie, BThistorique.FBT(i).FCommis,
                                           BThistorique.FBT(i).FVendeur, BThistorique.FBT(i).FEtat, BThistorique.FBT(i).FNotefinal, SCompagnie)

                For il As Integer = 0 To BThistorique.FBT(i).FTravailBT.Count - 1

                    Dim FLineNumber As String = Stga_to_Stgm(il)
                    Dim FUnite As String = Stga_to_Stgm(BThistorique.FBT(i).FUnite)
                    Dim BondeTravail As String = Stga_to_Stgm(BThistorique.FBT(i).FNoBT)
                    Dim Datedu As String = Stga_to_Stgm(BThistorique.FBT(i).FDate)
                    Dim Budget As String = Stga_to_Stgm(BThistorique.FBT(i).FEtat)
                    Dim FLettre As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FLettre)
                    Dim LettreTravail As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FLettre)
                    Dim FDescriptiontravail As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FDescriptiontravail)
                    Dim FEtat As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FEtat)
                    Dim employe As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FEmploye)
                    Dim FEmploye As String = Stga_to_Stgm(BThistorique.FBT(i).FTravailBT(il).FEmploye)


                    Dim success2 As Boolean = InsertOrUpdateBondeTravailJobs(BondeTravail, LettreTravail, FLineNumber, FEtat, FDescriptiontravail, employe, Datedu, Budget, FUnite, FClient, FNotefinal, SCompagnie)

                    For ili As Integer = 0 To BThistorique.FBT(i).FTravailBT(il).FitemsBT.Count - 1

                        Dim success3 As Boolean = InsertOrUpdateBondeTravail(BondeTravail & "-" & LettreTravail & "-" & BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FLineNumber, LettreTravail,
                                                                    BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FLineNumber, BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FPartnumber,
                                                                    BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FDescription, LettreTravail,
                                                                    BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FQte, BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FPrice,
                                                                    BThistorique.FBT(i).FTravailBT(il).FitemsBT(ili).FMontant, "",
                                                                    "", FEtat, FUnite, FClient, employe, SCompagnie)

                    Next
                Next

            Next

        Catch ex As Exception
            'Sresult1 = ex.Message
        End Try
    End Sub

#End Region


    Sub T_Add_Parts_Bondetravail(SCompagnie)
        Dim BondeTravail As String
        Dim ssortie As String = Lbldatesortie.InnerText.Trim
        Dim LettreTravail As String = btn_EntreSortie_employe_id.Text.Trim
        Dim FUnite As String = Lblunite_unite.InnerText.Trim
        Dim FClient As String = Fiche_Lbl_Client_id.InnerText.Trim
        Panel_Entrerdespieces_success.Visible = False
        Lbletatemploye.Text = ""
        Try

            BondeTravail = SortirJob_IDBondetravail.Value.Trim
            LettreTravail = BondeTravail.Substring(BondeTravail.Length - 1, 1)
            If ssortie <> "" Then
                Panel_Entrerdespieces_alert_lbl.InnerText = "Vous devez puncher l'employé !"
                Panel_Entrerdespieces_alert.Visible = True
                Exit Sub
            End If
            If BondeTravail = "" Then
                Panel_Entrerdespieces_alert_lbl.InnerText = "Vous devez être puncher sur un Bon de travail !"
                Panel_Entrerdespieces_alert.Visible = True
                Exit Sub
            End If
            If Add_Parts_id.Value.Trim = "" Then
                Panel_Entrerdespieces_alert_lbl.InnerText = "Vous devez inscrire un numéro de produit !"
                Panel_Entrerdespieces_alert.Visible = True
                Exit Sub
            End If
            If Add_Parts_id.Value.Trim <> "" Then
                LblmessagepucnhBT.InnerText = ""
                'Dim Scommentairevisible As String = TxtBoxvisble.Text.Trim
                'Dim ival As Integer = InStr(Scommentairevisible, vbCrLf)
                'If ival <> 0 Then Scommentairevisible = Replace(TxtBoxvisble.Text.Trim, vbCrLf, " ")
                'Dim Scommentairenonvisible As String = TxtBoxnonvisble.Text.Trim
                'ival = InStr(Scommentairenonvisible, vbCrLf)
                'If ival <> 0 Then Scommentairenonvisible = Replace(TxtBoxnonvisble.Text.Trim, vbCrLf, " ")
                If Add_Parts_qty.Value = "" Then
                    Add_Parts_qty.Value = "1"
                End If

                Dim Bpieceok As Boolean = validerpiece(Add_Parts_id.Value)
                If Bpieceok = True Then
                    Add_Parts_id.Value = ""
                    Panel_Entrerdespieces_success_lbl.InnerText = "La pièce a été ajouté"
                    Panel_Entrerdespieces_success.Visible = True
                    'punchPiece_(TxtBT.Text.Trim.PadRight(10) & Txtproduit.Text)
                    afficheinfo_Bontravail_parts("T_Add_Parts_Bondetravail", BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)

                    Panel_Entrerdespieces_alert.Visible = False
                    Panel_Entrerdespieces_success.Visible = True
                Else
                    Panel_Entrerdespieces_alert_lbl.InnerText = "Pieces inexistante"
                    Panel_Entrerdespieces_alert.Visible = True
                    Panel_Entrerdespieces_success.Visible = False

                End If
            End If


            Dim myScript As String = Add_Parts_id.Value & " " & Add_Parts_qty.Value

            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('" & myScript & "');", True)
        Catch ex As Exception

        End Try
    End Sub
    Sub T_Create_Job_M()
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim Scenario_ As String = "T_Create_Job_M"
        Dim FUnite As String = Lblunite_unite.InnerText
        Try
#Region "HEADER"

            Dim bvaliderpiece = False
            Dim BondeTravail = Lblbt.InnerText.Trim
            Dim LettreTravail As String = ""
            Dim Ires As Integer = InStr(BondeTravail, "-")
            If Ires <> 0 Then
                LettreTravail = ddl_Panel_Entrerdestravails_Step_1.Text.Trim
                BondeTravail = Mid(BondeTravail, 1, Ires - 1)
            End If
            Dim Sclient = Lblclient.InnerText.Trim
            Ires = InStr(Sclient, ":")
            If Ires <> 0 Then
                Sclient = Mid(Sclient, Ires + 1)
            End If
#End Region
            '   If Sbt.Trim <> "" And Scode.Trim <> "" And TxtQte.Text.Trim <> "" And Slettre.Trim <> "" Then
            slienemploye = slienip & "Getretourneinfolettrebt/" & Slienpath & LettreTravail.Trim.PadRight(1) &
                BondeTravail.Trim.PadRight(10) & Sclient.Trim.PadRight(7) & Create_Details_Job.Value.Trim
            Dim Sapi As String = GetAPIStringAsync(slienemploye)
            Try
                Dim reponse As String
                Sapi = Sapi
                If Sapi = Nothing Then Exit Sub
                reponse = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)
                Panel_Entrerdestravails_success_sublbl.InnerText = reponse
                If reponse.Trim = "Reussi" Then
                    Lblcommentaireajoutlettrebt.Text = "Ajout de job réussi"
                    Panel_Entrerdestravails_success.Visible = True
                    Panel_Entrerdestravails_success_lbl.InnerText = "Ajout de job réussi"
                    Create_Details_Job.Value = ""
                End If
            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try

            Dim myScript As String = Create_Details_Job.Value & " " & ddl_Panel_Entrerdestravails_Step_1.Text
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('" & myScript & "');", True)

            ' afficherBTemployeetjob("ZZ")
            afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
        Catch ex As Exception

        End Try
    End Sub

    Sub Reset_Creer_BT()
        Panel_CreationBT_1.Visible = False
        Panel_CreationBT_2.Visible = False
        Panel_CreationBT_3.Visible = False
        Panel_CreationBT_4.Visible = False
    End Sub

    Protected Sub btn_Panel_CreationBT_2_next_ServerClick(sender As Object, e As EventArgs)
        Reset_Creer_BT()
        Panel_CreationBT_3.Visible = True
    End Sub

    Protected Sub btn_Panel_CreationBT_2_ServerClick(sender As Object, e As EventArgs)
        Reset_Creer_BT()
        Panel_CreationBT_1.Visible = True
    End Sub

    Protected Sub btn_Panel_CreationBT_3_ServerClick(sender As Object, e As EventArgs)
        Reset_Creer_BT()
        Panel_CreationBT_2.Visible = True
    End Sub

    Protected Sub btn_Panel_CreationBT_4_ServerClick(sender As Object, e As EventArgs)
        Reset_Creer_BT()
        Panel_CreationBT_3.Visible = True
    End Sub

    Private Sub affiche_valeur_client()
        Reset_Creer_BT()
        Panel_CreationBT_2.Visible = True
        Panelclient.Visible = True
        Text4.Value = TextClientCreation.Value
        Try
            slienemploye = slienip.Trim & "Getretourneclientfixe/" & Slienpath & TextClientCreation.Value.Trim.PadRight(7)
            Try
                Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                client.KeepAlive = True
                Using WebResponse As HttpWebResponse = client.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Dim Clientcreationbt As requestclientSITEWEB = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of requestclientSITEWEB)(Sapi)

                    Lblnoclientct.Text = Clientcreationbt.client(0).noclient
                    Lblnomct.Text = Clientcreationbt.client(0).nom
                    Lbladrct.Text = Clientcreationbt.client(0).adr1
                    Lblcpct.Text = Clientcreationbt.client(0).cp1
                    Lbltelct.Text = Clientcreationbt.client(0).telcel

                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()
                End Using

            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try


        Catch ex As Exception

        End Try
    End Sub
    Private Sub afficher_Recherche_produit(ByVal SValeur As String)

        Try
            Dim Sapi As String
            'SValeur = UrlEncode(SValeur)
            SValeur = HttpUtility.UrlEncode(SValeur)
            'slienemploye = slienip.Trim & "Getretourneproduitapogeesql/" & Slienpath & "S" & SValeur
            slienemploye = slienip.Trim & "Getretourneproduitapogeesqlnew/resource?id=" & Slienpath & "S" & SValeur
            'slienemploye = slienip.Trim & "Getretourneproduitapogeesql/?param=" & Slienpath & "S" & SValeur
            'slienemploye = slienip.Trim & "Getretourneproduitapogeesql/" & Slienpath & "A" & SValeur
            Dim Rechercheproduit As DataTable
            Dim url As String = slienemploye

            Try

                ' Créer la requête
                Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                request.Method = "GET"

                ' Optionnel : Ajouter des en-têtes ou d'autres configurations à la requête
                ' request.Headers.Add("Authorization", "Bearer YOUR_TOKEN")
                Try
                    ' Envoyer la requête et obtenir la réponse
                    Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                        Using getreader As New StreamReader(response.GetResponseStream())
                            Sapi = getreader.ReadToEnd()
                            Rechercheproduit = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataTable)(Sapi)
                        End Using
                    End Using
                Catch ex As WebException
                    ' Gérer les erreurs
                    Console.WriteLine("Erreur lors de l'appel API: " & ex.Message)
                End Try

                Dim Get_Data_ As New DataTable
                Get_Data_.Columns.Add("CAT", GetType(String))
                Get_Data_.Columns.Add("ENT", GetType(String))
                Get_Data_.Columns.Add("DESCR", GetType(String))
                Get_Data_.Columns.Add("DESCA", GetType(String))
                Get_Data_.Columns.Add("TYPE", GetType(String))
                Get_Data_.Columns.Add("TYPEP", GetType(String))
                Get_Data_.Columns.Add("CATP", GetType(String))
                Get_Data_.Columns.Add("PRIX1", GetType(String))
                Get_Data_.Columns.Add("PRIX2", GetType(String))
                Get_Data_.Columns.Add("PRIX3", GetType(String))
                Get_Data_.Columns.Add("COUT", GetType(String))
                Get_Data_.Columns.Add("QMAIN", GetType(String))
                Get_Data_.Columns.Add("QCOM", GetType(String))
                Get_Data_.Columns.Add("QDISP", GetType(String))
                Get_Data_.Columns.Add("ENCOM", GetType(String))
                Get_Data_.Columns.Add("FOURN1", GetType(String))
                Get_Data_.Columns.Add("FOURN2", GetType(String))
                Get_Data_.Columns.Add("LOCAT", GetType(String))
                Get_Data_.Columns.Add("DIMMET", GetType(String))
                Get_Data_.Columns.Add("class_btn", GetType(String))
                Get_Data_.Columns.Add("class_QDISP", GetType(String))

                For Each row As DataRow In Rechercheproduit.Rows
                    Dim newRow As DataRow = Get_Data_.NewRow()

                    Dim filter_Rechercheproduit As Boolean = True
                    Dim CAT As String = row("CAT").ToString.Trim
                    Dim ENT As String = row("ENT").ToString.Trim
                    Dim DESCR As String = row("DESCR").ToString.Trim
                    Dim DESCA As String = row("DESCA").ToString.Trim
                    Dim TYPE As String = row("TYPE").ToString.Trim
                    Dim TYPEP As String = row("TYPEP").ToString.Trim
                    Dim CATP As String = row("CATP").ToString.Trim
                    Dim PRIX1 As String = row("PRIX1").ToString.Trim
                    Dim PRIX2 As String = row("PRIX2").ToString.Trim
                    Dim PRIX3 As String = row("PRIX3").ToString.Trim
                    Dim COUT As String = row("COUT").ToString.Trim
                    Dim QMAIN As String = row("QMAIN").ToString.Trim
                    Dim QCOM As String = row("QCOM").ToString.Trim
                    Dim ENCOM As String = row("ENCOM").ToString.Trim
                    Dim FOURN1 As String = row("FOURN1").ToString.Trim
                    Dim FOURN2 As String = row("FOURN2").ToString.Trim
                    Dim LOCAT As String = row("LOCAT").ToString.Trim
                    Dim DIMMET As String = row("DIMMET").ToString.Trim

                    Dim QDISP As String = row("QDISP").ToString.Trim
                    Dim double_QDISP As Double = Val(QDISP)
                    Dim class_QDISP As String = "text-danger"
                    Dim class_btn As String = "btn-light-facebook"

                    Select Case True
                        Case double_QDISP = 0
                            class_btn = "btn-light-youtube"
                            class_QDISP = "text-black"
                        Case double_QDISP < 0
                            class_btn = "btn-light-youtube"
                            class_QDISP = "text-black"
                    End Select

                    Select Case True
                        Case ddb_recherche_produit_filtre_inv.Text = "Tous"

                        Case ddb_recherche_produit_filtre_inv.Text = "Disponible"
                            Select Case True
                                Case double_QDISP = 0
                                    filter_Rechercheproduit = False
                                Case double_QDISP < 0
                                    filter_Rechercheproduit = False
                                Case Else
                                    double_QDISP = double_QDISP
                            End Select

                    End Select

                    If filter_Rechercheproduit = True Then
                        newRow("CAT") = CAT
                        newRow("ENT") = ENT
                        newRow("DESCR") = DESCR
                        newRow("DESCA") = DESCA
                        newRow("TYPE") = TYPE
                        newRow("TYPEP") = TYPEP
                        newRow("CATP") = CATP
                        newRow("PRIX1") = PRIX1
                        newRow("PRIX2") = PRIX2
                        newRow("PRIX3") = PRIX3
                        newRow("COUT") = COUT
                        newRow("QMAIN") = QMAIN
                        newRow("QCOM") = QCOM
                        newRow("QDISP") = QDISP
                        newRow("ENCOM") = ENCOM
                        newRow("FOURN1") = FOURN1
                        newRow("FOURN2") = FOURN2
                        newRow("LOCAT") = LOCAT
                        newRow("DIMMET") = DIMMET
                        newRow("class_QDISP") = class_QDISP
                        newRow("class_btn") = class_QDISP
                        Get_Data_.Rows.Add(newRow)
                    End If
                Next

                If Get_Data_.Rows.Count > 0 Then
                    Repeaterrechercheproduit.Visible = True
                    Repeaterrechercheproduit.DataSource = Get_Data_
                    Repeaterrechercheproduit.DataBind()
                    Lblnombre_de_produit.Text = "Nombre de produit retourné " & Get_Data_.Rows.Count
                Else
                    Repeaterrechercheproduit.Visible = False
                End If


            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try


        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try

        '    Dim Rechercheproduit As DataTable
        '    Dim Sapi As String
        'Try

        '    slienemploye = slienip.Trim & "Getretourneproduitapogeesql/" & Slienpath & "S" & SValeur
        '    Dim produit As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
        '    produit.KeepAlive = True
        '    Using WebResponse As HttpWebResponse = produit.GetResponse()
        '        Dim responseStream As Stream = WebResponse.GetResponseStream()
        '        Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
        '        Sapi = getreader.ReadToEnd()
        '        Rechercheproduit = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataTable)(Sapi)
        '        responseStream.Close()
        '        responseStream.Dispose()
        '        getreader.Close()
        '        getreader.Dispose()
        '        DtaClient.Rows.Clear()

        '    End Using

        '    If Sapi <> "null" Then
        '        Repeaterrechercheproduit.Visible = True
        '        Repeaterrechercheproduit.DataSource = Rechercheproduit
        '        Repeaterrechercheproduit.DataBind()
        '        Lblnombre_de_produit.Text = "Nombre de produit retourné " & Rechercheproduit.Rows.Count
        '    End If

        'Catch ex As Exception
        '    Dim Sresult1 As String = ex.Message
        'End Try


    End Sub
    Private Sub afficher_Recherche_client(ByVal SValeur As String)

        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Clients_web As String = Nothing,
            BondeTravail As String = Nothing,
            LettreTravail As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, "", Query_Langue, emailid, securitypin, semp, SCompagnie)

        Try
            'slienemploye = slienip.Trim & "Getretourneclientapogeesql/" & Slienpath & "A" & SValeur
            Dim surl As String = slienip.Trim & "Getretourneclientapogeesql/" & Slienpath & "S" & SValeur
            Try
                Dim Rechercheclient As DataTable
                Dim Sapi As String
                Dim produit As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(surl.Trim), HttpWebRequest)
                produit.KeepAlive = True
                Using WebResponse As HttpWebResponse = produit.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Sapi = getreader.ReadToEnd()
                    Rechercheclient = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataTable)(Sapi)
                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()
                End Using

                If Sapi <> "null" Then
                    Repeaterrechercheclient.Visible = True
                    Repeaterrechercheclient.DataSource = Rechercheclient
                    Repeaterrechercheclient.DataBind()
                    lblnombre_de_client.Text = "Nombre de client retoutné " & Rechercheclient.Rows.Count

                    For Each row As DataRow In Rechercheclient.Rows
                        ' Traitez chaque ligne ici
                        ' Par exemple, pour afficher chaque valeur de la première colonne :
                        Dim valeur As String = row(0).ToString()
                        Dim Clients_noclient As String = row("noclient").ToString()
                        Dim Clients_autrea As String = row("autrea").ToString()
                        Dim Clients_comp As String = row("nom").ToString()
                        Dim Clients_contact As String = row("contact").ToString()
                        Dim Clients_groupe As String = row("groupe").ToString()
                        Dim Clients_famille As String = row("typep").ToString()
                        Dim Clients_type As String = row("type").ToString()
                        Dim Clients_category As String = row("cat").ToString()
                        Dim Fidelity_id As String = row("loyalid").ToString()

                        Dim Clients_birthday As String = row("birthday").ToString()
                        Dim Clients_addr_1 As String = row("adr1").ToString()
                        Dim Clients_cp_1 As String = row("cp1").ToString()
                        Dim Clients_ville_1 As String = row("ville").ToString()
                        Dim Clients_prov_1 As String = row("prov").ToString()
                        Dim Clients_pays_1 As String = row("pays").ToString()

                        Dim Clients_territoire As String = row("ter").ToString()

                        Dim Clients_email As String = row("email").ToString()

                        Dim Clients_fax As String = row("tphone").ToString()
                        'Dim Clients_fax As String = row("teladd").ToString()
                        Dim Clients_phone_contact As String = row("telcon").ToString()
                        Dim Clients_telcel As String = row("telcel").ToString()
                        ' Faites quelque chose avec la valeur

                        Creer_Clients_MySql(Clients_comp, Clients_noclient, Clients_contact, Clients_groupe,
          Clients_famille, Clients_type, Clients_category, Clients_addr_1, Fidelity_id, Nothing,
          Clients_cp_1, Clients_ville_1, Clients_prov_1, Clients_pays_1, Clients_phone_contact,
          Clients_telcel, Clients_fax, Clients_email, Clients_birthday, Clients_autrea, Clients_territoire, SCompagnie)


                    Next

                End If





            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try
        Catch ex As Exception

        End Try
    End Sub

    ' Déclare la liste pour stocker les paires (champs, description)
    Private myList As New List(Of ChampDescription)()

    ' Fonction pour ajouter une chaîne à la liste si elle n'est pas Nothing
    Protected Sub AddStringPairAndBindToRepeater(ByVal champ As String, ByVal description As String)
        If Not String.IsNullOrEmpty(champ) AndAlso Not String.IsNullOrEmpty(description) Then
            myList.Add(New ChampDescription With {.Champ = champ, .Description = description})
        End If

    End Sub

    Private Sub Affiche_valeur_unite(Lblunite_, scenario_)
        Dim Sapi As String
        Dim Lblunite_String As String = Lblunite_
        Lblunite_String = Lblunite_String.PadRight(30)
        Try
            slienemploye = slienip.Trim & "Getretournevehiculesiteweb/" & Slienpath & Lblunite_String & Lblunite_String
            Sapi = GetAPIStringAsync(slienemploye)
            Dim Vehiculecreationbt As requestvehiculeSITEWEB = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of requestvehiculeSITEWEB)(Sapi)

            Dim unite As String = Vehiculecreationbt.vehicule(0).unite
            Dim descr_unite As String = "#ID Vehicule"

            Dim ser2 As String = Vehiculecreationbt.vehicule(0).ser2
            Dim descr_ser2 As String = "Serie 2"

            Dim ser3 As String = Vehiculecreationbt.vehicule(0).ser3
            Dim descr_ser3 As String = "Serie 3"

            Dim ser4 As String = Vehiculecreationbt.vehicule(0).ser4
            Dim descr_ser4 As String = "Serie 4"

            Dim serie As String = Vehiculecreationbt.vehicule(0).serie
            Dim descr_serie As String = "Serie"

            Dim annee As String = Vehiculecreationbt.vehicule(0).annee
            Dim descr_annee As String = "Annee"

            Dim aut1 As String = Vehiculecreationbt.vehicule(0).aut1
            Dim descr_aut1 As String = Vehiculecreationbt.vehicule(0).aut1
            Dim aut2 As String = Vehiculecreationbt.vehicule(0).aut2
            Dim descr_aut2 As String = Vehiculecreationbt.vehicule(0).aut2
            Dim aut3 As String = Vehiculecreationbt.vehicule(0).aut3
            Dim descr_aut3 As String = Vehiculecreationbt.vehicule(0).aut3
            Dim aut4 As String = Vehiculecreationbt.vehicule(0).aut4
            Dim descr_aut4 As String = Vehiculecreationbt.vehicule(0).aut4
            Dim aut5 As String = Vehiculecreationbt.vehicule(0).aut5
            Dim descr_aut5 As String = Vehiculecreationbt.vehicule(0).aut5
            Dim aut6 As String = Vehiculecreationbt.vehicule(0).aut6
            Dim descr_aut6 As String = Vehiculecreationbt.vehicule(0).aut6
            Dim aut7 As String = Vehiculecreationbt.vehicule(0).aut7
            Dim descr_aut7 As String = Vehiculecreationbt.vehicule(0).aut7
            Dim aut8 As String = Vehiculecreationbt.vehicule(0).aut8
            Dim descr_aut8 As String = Vehiculecreationbt.vehicule(0).aut8
            Dim aut9 As String = Vehiculecreationbt.vehicule(0).aut9
            Dim descr_aut9 As String = Vehiculecreationbt.vehicule(0).aut9
            Dim aut10 As String = Vehiculecreationbt.vehicule(0).aut10
            Dim descr_aut10 As String = Vehiculecreationbt.vehicule(0).aut10

            Dim boit1 As String = Vehiculecreationbt.vehicule(0).boit1
            Dim descr_boit1 As String = Vehiculecreationbt.vehicule(0).boit1
            Dim boit2 As String = Vehiculecreationbt.vehicule(0).boit2
            Dim descr_boit2 As String = Vehiculecreationbt.vehicule(0).boit2
            Dim boit3 As String = Vehiculecreationbt.vehicule(0).boit3
            Dim descr_boit3 As String = Vehiculecreationbt.vehicule(0).boit3
            Dim boit4 As String = Vehiculecreationbt.vehicule(0).boit4
            Dim descr_boit4 As String = Vehiculecreationbt.vehicule(0).boit4
            Dim boit5 As String = Vehiculecreationbt.vehicule(0).boit5
            Dim descr_boit5 As String = Vehiculecreationbt.vehicule(0).boit5
            Dim boit6 As String = Vehiculecreationbt.vehicule(0).boit6
            Dim descr_boit6 As String = Vehiculecreationbt.vehicule(0).boit6
            Dim boit7 As String = Vehiculecreationbt.vehicule(0).boit7
            Dim descr_boit7 As String = Vehiculecreationbt.vehicule(0).boit7
            Dim boit8 As String = Vehiculecreationbt.vehicule(0).boit8
            Dim descr_boit8 As String = Vehiculecreationbt.vehicule(0).boit8
            Dim boit9 As String = Vehiculecreationbt.vehicule(0).boit9
            Dim descr_boit9 As String = Vehiculecreationbt.vehicule(0).boit9
            Dim boit10 As String = Vehiculecreationbt.vehicule(0).boit10
            Dim descr_boit10 As String = Vehiculecreationbt.vehicule(0).boit10


            Dim cabine As String = Vehiculecreationbt.vehicule(0).cabine
            Dim descr_cabine As String = "Cabine"
            Dim client As String = Vehiculecreationbt.vehicule(0).client
            Dim descr_client As String = "Client"
            Dim desactiver As String = Vehiculecreationbt.vehicule(0).desactiver
            Dim descr_desactiver As String = "Desactiver"
            Dim descr As String = Vehiculecreationbt.vehicule(0).descr
            Dim descr_descr As String = "Description"
            Dim groupe As String = Vehiculecreationbt.vehicule(0).groupe
            Dim descr_groupe As String = "Groupe"
            Dim image As String = Vehiculecreationbt.vehicule(0).image
            Dim descr_image As String = "Image"
            Dim interne As String = Vehiculecreationbt.vehicule(0).interne
            Dim descr_interne As String = "Interne"
            Dim liendetail As String = Vehiculecreationbt.vehicule(0).liendetail
            Dim descr_liendetail As String = Vehiculecreationbt.vehicule(0).liendetail


            Dim manufactur As String = Vehiculecreationbt.vehicule(0).manufactur
            Dim descr_manufactur As String = "Manufacturier"
            Dim marque As String = Vehiculecreationbt.vehicule(0).marque
            Dim descr_marque As String = "Marque"
            Dim modele As String = Vehiculecreationbt.vehicule(0).modele
            Dim descr_modele As String = "Modele"
            Dim nbessieu As String = Vehiculecreationbt.vehicule(0).nbessieu
            Dim descr_nbessieu As String = "Nb. Essieux"
            Dim nbrheure As String = Vehiculecreationbt.vehicule(0).nbrheure
            Dim descr_nbrheure As String = "Nb. Heure"
            Dim options As String = Vehiculecreationbt.vehicule(0).options
            Dim descr_options As String = "Options"
            Dim poids As String = Vehiculecreationbt.vehicule(0).poids
            Dim descr_poids As String = "Poids"
            Dim prixus As String = Vehiculecreationbt.vehicule(0).prixus
            Dim descr_prixus As String = "Prix Us"


            myList.Clear()

            ' Ajout des chaînes à la liste et liaison au Repeater
            AddStringPairAndBindToRepeater(unite, descr_unite)
            AddStringPairAndBindToRepeater(ser2, descr_ser2)
            AddStringPairAndBindToRepeater(ser3, descr_ser3)
            AddStringPairAndBindToRepeater(ser4, descr_ser4)
            AddStringPairAndBindToRepeater(serie, descr_serie)
            AddStringPairAndBindToRepeater(annee, descr_annee)
            AddStringPairAndBindToRepeater(aut1, descr_aut1)
            AddStringPairAndBindToRepeater(aut2, descr_aut2)
            AddStringPairAndBindToRepeater(aut3, descr_aut3)
            AddStringPairAndBindToRepeater(aut4, descr_aut4)
            AddStringPairAndBindToRepeater(aut5, descr_aut5)
            AddStringPairAndBindToRepeater(aut6, descr_aut6)
            AddStringPairAndBindToRepeater(aut7, descr_aut7)
            AddStringPairAndBindToRepeater(aut8, descr_aut8)
            AddStringPairAndBindToRepeater(aut9, descr_aut9)
            AddStringPairAndBindToRepeater(aut10, descr_aut10)
            AddStringPairAndBindToRepeater(boit1, descr_boit1)
            AddStringPairAndBindToRepeater(boit2, descr_boit2)
            AddStringPairAndBindToRepeater(boit3, descr_boit3)
            AddStringPairAndBindToRepeater(boit4, descr_boit4)
            AddStringPairAndBindToRepeater(boit5, descr_boit5)
            AddStringPairAndBindToRepeater(boit6, descr_boit6)
            AddStringPairAndBindToRepeater(boit7, descr_boit7)
            AddStringPairAndBindToRepeater(boit8, descr_boit8)
            AddStringPairAndBindToRepeater(boit9, descr_boit9)
            AddStringPairAndBindToRepeater(boit10, descr_boit10)
            AddStringPairAndBindToRepeater(cabine, descr_cabine)
            AddStringPairAndBindToRepeater(client, descr_client)
            AddStringPairAndBindToRepeater(desactiver, descr_desactiver)
            AddStringPairAndBindToRepeater(descr, descr_descr)
            AddStringPairAndBindToRepeater(groupe, descr_groupe)
            'AddStringPairAndBindToRepeater(image, descr_image)
            AddStringPairAndBindToRepeater(interne, descr_interne)
            'AddStringPairAndBindToRepeater(liendetail, descr_liendetail)
            'AddStringPairAndBindToRepeater(manufactur, descr_manufactur)
            AddStringPairAndBindToRepeater(marque, descr_marque)
            AddStringPairAndBindToRepeater(modele, descr_modele)
            AddStringPairAndBindToRepeater(nbessieu, descr_nbessieu)
            AddStringPairAndBindToRepeater(nbrheure, descr_nbrheure)
            AddStringPairAndBindToRepeater(options, descr_options)
            AddStringPairAndBindToRepeater(poids, descr_poids)
            AddStringPairAndBindToRepeater(prixus, descr_prixus)


            ' Lier la liste au Repeater
            Repeater_Panel_details_veh_detail.DataSource = myList
            Repeater_Panel_details_veh_detail.DataBind()

            Select Case True
                Case scenario_ = "VALIDER"
                    Lblunitect.Text = Vehiculecreationbt.vehicule(0).unite
                    Lblmarquect.Text = Vehiculecreationbt.vehicule(0).marque
                    Lblmodelect.Text = Vehiculecreationbt.vehicule(0).modele
                    Lblanneect.Text = Vehiculecreationbt.vehicule(0).annee
                    Lblseriect.Text = Vehiculecreationbt.vehicule(0).serie


                Case scenario_ = "TEMPS"

                    Lblunite_unite.InnerText = Vehiculecreationbt.vehicule(0).unite
                    Lblunite_ser2.InnerText = Vehiculecreationbt.vehicule(0).serie
                    Lblunite_modele.InnerText = Vehiculecreationbt.vehicule(0).annee & " " & Vehiculecreationbt.vehicule(0).marque & " " & Vehiculecreationbt.vehicule(0).modele


            End Select
        Catch ex As Exception

        End Try
    End Sub
    Private Sub afficher_Recherche_unite(ByVal SValeur As String)
        Try
            Dim Rechercheunite As DataTable
            'slienemploye = slienip.Trim & "Getretourneuniteapogeesql/" & Slienpath & "A" & SValeur
            slienemploye = slienip.Trim & "Getretourneuniteapogeesql/" & Slienpath & "S" & SValeur
            Try
                Dim produit As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
                produit.KeepAlive = True
                Using WebResponse As HttpWebResponse = produit.GetResponse()
                    Dim responseStream As Stream = WebResponse.GetResponseStream()
                    Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                    Dim Sapi As String = getreader.ReadToEnd()
                    Rechercheunite = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataTable)(Sapi)
                    responseStream.Close()
                    responseStream.Dispose()
                    getreader.Close()
                    getreader.Dispose()

                End Using

                Repeaterrechercheunite.Visible = True
                'Dim Sval As String = TextClientCreation.Value.Trim
                ''   Dim strFiltre As String = "[" & "CLIENT" & "]" & " LIKE '%" & Sval.Trim & "%'"
                'Dim filteredRows As DataRow() = Rechercheunite.Select("CLIENT = INTERNE")

                'Repeaterrechercheunite.DataSource = filteredRows


                Repeaterrechercheunite.DataSource = Rechercheunite
                Repeaterrechercheunite.DataBind()
                lblnombre_de_unite.Text = "Nombre d'unité retoutné " & Rechercheunite.Rows.Count

            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try


        Catch ex As Exception

        End Try
    End Sub
    Function Creer_Client(Clients_id, Fidelity_id, Clients_email, semp, SCompagnie) As String
        Try
            Dim Scenario_ As String = Nothing, Clients_comp As String = Nothing, Clients_contact As String = Nothing, Clients_groupe As String = Nothing,
            Clients_famille As String = Nothing, Clients_type As String = Nothing, Clients_category As String = Nothing, Clients_addr_1 As String = Nothing, Clients_admin As String = Nothing,
            Clients_cp_1 As String = Nothing, Clients_ville_1 As String = Nothing, Clients_prov_1 As String = Nothing, Clients_pays_1 As String = Nothing, Clients_phone_contact As String = Nothing,
            Clients_telcel As String = Nothing, Clients_fax As String = Nothing, Clients_birthday As String = Nothing, Clients_autrea As String = Nothing, Clients_territoire As String = Nothing
            Dim Clients_genre = Nothing, Clients_sexe = Nothing
            Dim list_ As New List(Of String)
            Dim values_ As New ArrayList()

            Dim Query_Langue As String = Nothing

            Dim fname_ As String = tb_fname.Value
            Dim lname_ As String = tb_lname.Value
            Clients_comp = lname_ & "," & fname_
            Clients_addr_1 = Signup_step_member_Clients_addr_1.Value
            Clients_ville_1 = Signup_step_member_Clients_ville_1.Value
            Clients_cp_1 = Signup_step_member_Clients_cp_1.Value
            Clients_prov_1 = Signup_step_member_Clients_prov.Value
            Clients_type = Signup_step_member_Clients_type.Text
            Clients_telcel = tb_phone.Value
            Clients_birthday = Signup_step_timezone.Value & " % " & Signup_step_member_Langue.Text & " % " & Signup_step_currnecy.Text

            Clients_pays_1 = "CA"
            Scenario_ = "CLIENT"
            Dim DBSelection As String = "clients"
            Dim FieldVariable_ As String = "Clients_id"

            Select Case True
                Case Clients_id = Nothing
                    Find_Next_Entry_Clients(Clients_id)
            End Select

            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_email, "Clients_email", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Fidelity_id, "Fidelity_id", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, UMan_API_Name, "Clients_Plateforme", DefaultDataBase)

            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_comp, "Clients_comp", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_type, "Clients_type", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_addr_1, "Clients_addr_1", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_ville_1, "Clients_ville_1", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_cp_1, "Clients_cp_1", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_prov_1, "Clients_prov_1", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_telcel, "Clients_telcel", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_email, "Clients_email", DefaultDataBase)
            Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_birthday, "Clients_birthday", DefaultDataBase)
            Load_Cookie("WRITE", Clients_id, "999999", Fidelity_id, Signup_step_member_Langue.Text, Clients_email, "", semp, SCompagnie)

            Show_Online_Client(Clients_id, Fidelity_id, Clients_comp, Clients_addr_1)

        Catch ex As Exception
            Clients_id = Nothing
        End Try
        Return Clients_id
    End Function

    Function Search_And_Create_Clients(scenario_, Lblclient_, SCompagnie)
        slienemploye = slienip & "Getretourneclientfixe/" & Slienpath & Lblclient_.Trim.PadRight(7) & TextClientCreation.Value.Trim.PadRight(7)
        Dim Clientcreationbt As requestclientSITEWEB
        Dim Sapi As String = GetAPIStringAsync(slienemploye)
        Sapi = Sapi
        If Sapi = Nothing Then Exit Function
        Clientcreationbt = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of requestclientSITEWEB)(Sapi)

        Dim Clients_noclient As String = Clientcreationbt.client(0).noclient
        Dim noclient As String = Clientcreationbt.client(0).noclient
        Dim descr_noclient As String = "# Client"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(noclient, descr_noclient)
        End If

        Dim nom As String = Clientcreationbt.client(0).nom
        Dim Clients_comp As String = Clientcreationbt.client(0).nom
        Dim descr_nom As String = "Nom"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(nom, descr_nom)
        End If

        Dim Clients_contact As String = Clientcreationbt.client(0).contact
        Dim contact As String = Clientcreationbt.client(0).contact
        Dim descr_contact As String = "Contact"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(contact, descr_contact)
        End If

        Dim groupe As String = Clientcreationbt.client(0).groupe
        Dim Clients_groupe As String = Clientcreationbt.client(0).groupe
        Dim descr_groupe As String = "Groupe"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(groupe, descr_groupe)
        End If

        Dim Clients_type As String = Clientcreationbt.client(0).type
        Dim type As String = Clientcreationbt.client(0).type
        Dim descr_type As String = "Type"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(type, descr_type)
        End If

        Dim Clients_category As String = Clientcreationbt.client(0).cat
        Dim cat As String = Clientcreationbt.client(0).cat
        Dim descr_cat As String = "Categorie"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(cat, descr_cat)
        End If

        Dim Fidelity_id As String = Clientcreationbt.client(0).loyalid
        Dim loyalid As String = Clientcreationbt.client(0).loyalid
        Dim descr_loyalid As String = "# Membre"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(loyalid, descr_loyalid)
        End If

        Dim Clients_autrea As String = Clientcreationbt.client(0).autrea
        Dim autrea As String = Clientcreationbt.client(0).autrea
        Dim descr_autrea As String = "Autre"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(autrea, descr_autrea)
        End If

        Dim Clients_birthday As String = Clientcreationbt.client(0).birthday
        Dim birthday As String = Clientcreationbt.client(0).birthday
        Dim descr_birthday As String = "Fete"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(birthday, descr_birthday)
        End If

        Dim Clients_addr_1 As String = Clientcreationbt.client(0).adr1
        Dim adr1 As String = Clientcreationbt.client(0).adr1
        Dim descr_adr1 As String = "Addresse 1"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(adr1, descr_adr1)
        End If

        Dim Clients_cp_1 As String = Clientcreationbt.client(0).cp1
        Dim cp1 As String = Clientcreationbt.client(0).cp1
        Dim descr_cp1 As String = "Code Postal"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(cp1, descr_cp1)
        End If
        Dim Clients_ville_1 As String = Clientcreationbt.client(0).ville
        Dim ville As String = Clientcreationbt.client(0).ville
        Dim descr_ville As String = "Ville"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(ville, descr_ville)
        End If

        Dim Clients_prov_1 As String = Clientcreationbt.client(0).prov
        Dim prov As String = Clientcreationbt.client(0).prov
        Dim descr_prov As String = "Province"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(prov, descr_prov)
        End If

        Dim Clients_pays_1 As String = Clientcreationbt.client(0).pays
        Dim pays As String = Clientcreationbt.client(0).pays
        Dim descr_pays As String = "Pays"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(pays, descr_pays)
        End If

        Dim adr2 As String = Clientcreationbt.client(0).adr2
        Dim descr_adr2 As String = "Addresse 2"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(adr2, descr_adr2)
        End If

        Dim tphone As String = Clientcreationbt.client(0).tphone
        Dim descr_tphone As String = "Telephone"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(tphone, descr_tphone)
        End If

        Dim teladd As String = Clientcreationbt.client(0).teladd
        Dim descr_teladd As String = "Tel. Add."
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(teladd, descr_teladd)
        End If

        Dim Clients_phone_contact As String = Clientcreationbt.client(0).telcon
        Dim telcon As String = Clientcreationbt.client(0).telcon
        Dim descr_telcon As String = "Tel. Contact"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(telcon, descr_telcon)
        End If

        Dim Clients_telcel As String = Clientcreationbt.client(0).telcel
        Dim telcel As String = Clientcreationbt.client(0).telcel
        Dim descr_telcel As String = "Cellulaire"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(noclient, descr_noclient)
        End If

        Dim Clients_email As String = Clientcreationbt.client(0).email
        Dim email As String = Clientcreationbt.client(0).email
        Dim descr_email As String = "Courriel"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(email, descr_email)
        End If

        Dim Clients_fax As String = Clientcreationbt.client(0).fax
        Dim fax As String = Clientcreationbt.client(0).fax
        Dim descr_fax As String = "Fax"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(fax, descr_fax)
        End If

        Dim motcle1 As String = Clientcreationbt.client(0).motcle1
        Dim descr_motcle1 As String = "Mot. Clef 1"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(motcle1, descr_motcle1)
        End If

        Dim motcle2 As String = Clientcreationbt.client(0).motcle2
        Dim descr_motcle2 As String = "Mot. Clef 2"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(motcle2, descr_motcle2)
        End If

        Dim Clients_territoire As String = Clientcreationbt.client(0).ter
        Dim ter As String = Clientcreationbt.client(0).ter
        Dim descr_ter As String = "Territoire"
        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            AddStringPairAndBindToRepeater(ter, descr_ter)
        End If

        If scenario_ = "btn_Panel_PreFacture_ServerClick" Then

            Dim BondeTravail As String = Badge_Status_WAIT.InnerText
            Dim LettreTravail As String = Nothing
            Load_Bon_Travail_Info(BondeTravail, LettreTravail)
            Dim FGtotal As String = GetColumnValueFromDB("fbt", "FNoBT", BondeTravail, "FGtotal", SCompagnie)
            Dim FStotal As String = Nothing
            Dim FTPS As String = Nothing
            Dim FTVQ As String = Nothing
            Dim FLivr As String = Nothing
            Function_PreFacturation(FGtotal, FStotal, FTPS, FTVQ, FLivr)

            Panel_PreFacture_STotal.InnerHtml = FStotal & "$"
            Panel_PreFacture_TPS.InnerHtml = FTPS & "$"
            Panel_PreFacture_TVQ.InnerHtml = FTVQ & "$"
            Panel_PreFacture_FLivr.InnerHtml = FLivr & "$"
            Panel_PreFacture_GTotal.InnerHtml = FGtotal & "$"

            Panel_PreFacture_Addr_Fact.InnerHtml = adr1 & "<br />" & Clients_ville_1 & "<br />" & Clients_cp_1 & "<br />" & Clients_prov_1 & " " & Clients_pays_1
            Panel_PreFacture_Addr_Livr.InnerHtml = adr1 & "<br />" & Clients_ville_1 & "<br />" & Clients_cp_1 & "<br />" & Clients_prov_1 & " " & Clients_pays_1


            Try
                Dim Get_Data_2 As DataTable = GetData("Search_And_Create_Clients", "bondetravailjobs", " WHERE bondetravailjobs_ID LIKE '%" & BondeTravail & "%' ORDER BY LettreTravail ASC", 1, 10000)
                If Get_Data_2.Rows.Count > 0 Then
                    ViewState("dt") = Get_Data_2
                    Repeater_Panel_PreFacture_Jobs.DataSource = Get_Data_2
                    Repeater_Panel_PreFacture_Jobs.DataBind()
                End If
            Catch ex As Exception

            End Try


            Try
                Dim Get_Data_2 As DataTable = GetData("Search_And_Create_Clients", "bondetravailpiece", " WHERE BondeTravail LIKE '%" & BondeTravail & "%' ORDER BY LettreTravail ASC", 1, 10000)
                If Get_Data_2.Rows.Count > 0 Then
                    ViewState("dt") = Get_Data_2
                    Repeater_Panel_PreFacture_Pieces.DataSource = Get_Data_2
                    Repeater_Panel_PreFacture_Pieces.DataBind()
                End If
            Catch ex As Exception

            End Try


        End If

        Creer_Clients_MySql(Clients_comp, Clients_noclient, Clients_contact, Clients_groupe,
          Nothing, Clients_type, Clients_category, Clients_addr_1, Fidelity_id, Nothing,
          Clients_cp_1, Clients_ville_1, Clients_prov_1, Clients_pays_1, Clients_phone_contact,
          Clients_telcel, Clients_fax, Clients_email, Clients_birthday, Clients_autrea, Clients_territoire, SCompagnie)


        Select Case True
            Case scenario_ = "Btn_val_client_ServerClick"
                Panelclient.Visible = True
                Lblnoclientct.Text = noclient
                Lblnomct.Text = nom
                Lbladrct.Text = Clientcreationbt.client(0).adr1
                Lblcpct.Text = Clientcreationbt.client(0).cp1
                Lbltelct.Text = telcel
            Case Else

                Panel_PreFacture_Client_nom.InnerHtml = nom
                Fiche_Lblnomct.InnerHtml = nom
                Panel_PreFacture_Client_email.InnerHtml = email
                Fiche_Lblemail.InnerHtml = email
                Fiche_Lblemail.HRef = "mailto:" & email
                Fiche_Lbltelct.InnerHtml = telcel
                Fiche_Lbltelct.HRef = "tel:" & telcel
        End Select


    End Function
    Sub Creer_Clients_MySql(Clients_comp, Clients_noclient, Clients_contact, Clients_groupe,
          Clients_famille, Clients_type, Clients_category, Clients_addr_1, Fidelity_id, Clients_admin,
          Clients_cp_1, Clients_ville_1, Clients_prov_1, Clients_pays_1, Clients_phone_contact,
          Clients_telcel, Clients_fax, Clients_email, Clients_birthday, Clients_autrea, Clients_territoire, Clients_Plateforme)
        Dim Clients_id = Nothing, Clients_genre = Nothing, Clients_sexe = Nothing
        Dim Scenario_ As String = Nothing
        Dim list_ As New List(Of String)
        Dim values_ As New ArrayList()
        Dim Query_Langue As String = Nothing

        Scenario_ = "CLIENT"
        Dim DBSelection As String = "clients"
        Dim FieldVariable_ As String = "Clients_id"

        Select Case True
            Case Process_Count_Mysql("", "clients", " WHERE Clients_noclient = '" & Clients_noclient & "'") > 0
                Clients_id = GetColumnValueFromDB("clients", "Clients_noclient", Clients_noclient, "Clients_id", "GSIGSI")
            Case Clients_id = Nothing
                Find_Next_Entry_Clients(Clients_id)
        End Select

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_noclient, "Clients_noclient", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_autrea, "Clients_autrea", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_Plateforme, "Clients_Plateforme", DefaultDataBase)

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Fidelity_id, "Fidelity_id", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_comp, "Clients_comp", DefaultDataBase)

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_groupe, "Clients_groupe", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_famille, "Clients_famille", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_type, "Clients_type", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_category, "Clients_category", DefaultDataBase)

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_addr_1, "Clients_addr_1", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_ville_1, "Clients_ville_1", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_cp_1, "Clients_cp_1", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_prov_1, "Clients_prov_1", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_pays_1, "Clients_pays_1", DefaultDataBase)

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_telcel, "Clients_telcel", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_fax, "Clients_fax", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_phone_contact, "Clients_phone_contact", DefaultDataBase)

        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_email, "Clients_email", DefaultDataBase)
        Where_To_Update(DBSelection, FieldVariable_, Clients_id, Clients_birthday, "Clients_birthday", DefaultDataBase)
    End Sub
    Public Function punch(semp As String, SBT As String, Scommantairevisible As String, SCommentaireinv As String, Setat As String) As String
#Region "API punch Employe"
        Dim reponse As String = Nothing
        'Dim semp As String = btn_EntreSortie_employe_id.Text
        Dim Ires As Integer = InStr(semp, "(")
        If Ires <> 0 Then
            semp = Mid(semp, 1, Ires - 2)
        End If
        ' semp = "=" & semp.Trim
        semp = semp.Trim.PadRight(30)
        Dim Sparametre As String = "?id=" & Slienpath & "&Semp=" & semp & "&SBT=" & SBT.PadLeft(10) & "&Scommantairevisible=" & Scommantairevisible & "&SCommentaireinv=" & SCommentaireinv & "&Setat=" & Setat

        slienemploye = slienip & "GetProcessPunchBTemploye/resource" & Sparametre
        Try
            Dim client As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(slienemploye.Trim), HttpWebRequest)
            client.KeepAlive = True
            Dim Sapi As String
            Using WebResponse As HttpWebResponse = client.GetResponse()
                Dim responseStream As Stream = WebResponse.GetResponseStream()
                Dim getreader As StreamReader = New StreamReader(responseStream, Encoding.Default)
                Sapi = getreader.ReadToEnd()
                responseStream.Close()
                responseStream.Dispose()
                getreader.Close()
                getreader.Dispose()
            End Using
            reponse = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of String)(Sapi)


        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try

        Return reponse
#End Region
    End Function
    Function afficher_BT_Jobs_Apogee(semp) As String
        semp = semp.Trim.PadRight(30)
        slienemploye = slienip & "GetretourneBondeTravailpunch/" & Slienpath & semp
        Dim Sapi As String = GetAPIStringAsync(slienemploye)
        Return Sapi
    End Function
    Sub afficher_BT_Jobs(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
        Dim semp As String = btn_EntreSortie_employe_id.Text.Trim
        Dim FClient As String = Fiche_Lbl_Client_id.InnerText.Trim
        Dim ini_Scenario_ As String
        Dim Sapi As String

        Try
            Dim Dict_afficher_BT_Jobs As New Dictionary(Of String, String) From {
      {"afficheinfo_Bontravail_emp_Scenario_3", "ALL"},
      {"btn_Panel_Entrerdestravails_ServerClick", "ALL"},
      {"Filter_Letter_Tasks", "ALL"},
      {"Filter_Letter_Parts", "ALL"},
      {"T_Create_Job_M", "ALL"},
      {"btn_offline_panel_AllJobs_ServerClick", "OFFLINEALL"},
      {"btn_BT_en_cours_employe_complet_ServerClick1", "EMPALL"},
      {"btn_wo_actuel_ServerClick", "EMPBT"},
      {"afficheinfo_Bontravail_emp_Scenario_2", "EMPBT"},
      {"afficheinfo_Bontravail_emp_Scenario_1", "EMPBT"},
      {"btn_offline_panel_Jobs_ServerClick", "EMPBT"},
      {"btn_Panel_Bontravail_ServerClick", "BT"},
      {"btn_Panel_Entrerdespieces_ServerClick", "BT"},
      {"KeyEND", "Value3"}
  }

            ini_Scenario_ = GetValueIfKeyExists(scenario_, Dict_afficher_BT_Jobs)
        Catch ex As Exception
            scenario_ = scenario_
            Exit Sub
        End Try
        Try
            Dim Dict_afficher_BT_Jobs As New Dictionary(Of String, String) From {
      {"ZZ", "ZZ"},
      {"ALL", "ALL"},
      {"BT", "BT"},
      {"EMPALL", "EMPALL"},
      {"EMPBT", "EMPBT"},
      {"OFFLINEALL", "OFFLINEALL"},
      {"KeyEND", "Value3"}
  }

            ini_Scenario_ = GetValueIfKeyExists(ini_Scenario_, Dict_afficher_BT_Jobs)
        Catch ex As Exception
            scenario_ = scenario_
            Exit Sub
        End Try

        Select Case ini_Scenario_
            Case "Key not found in dictionary."
                scenario_ = scenario_
                ini_Scenario_ = ini_Scenario_
                afficher_BT_Jobs(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                Exit Sub
            Case "ZZ"
                Try
                    Dim parts As String() = semp.Split(New String() {" ("}, StringSplitOptions.None)
                    semp = parts(0)
                Catch ex As Exception

                End Try
            Case "ALL", "BT", "EMPALL", "OFFLINEALL"
                semp = "ZZ"
            Case "EMPBT"
                Dim ires As Integer = 0
                ires = InStr(semp, "(")
                If ires <> 0 Then
                    semp = Mid(semp, 1, ires - 2)
                End If
        End Select
        Sapi = afficher_BT_Jobs_Apogee(semp)


        scenario_ = scenario_
        ini_Scenario_ = ini_Scenario_
        Select Case ini_Scenario_
            Case "BT_en_cours_employe_complet"
                afficheinfo_all_wo(scenario_, Sapi, "ALL", "BT", SCompagnie)
            Case "OFFLINEALL"
                afficheinfo_all_wo(scenario_, Sapi, "ALL", "ALL", SCompagnie)
            Case "ALL"
                Select Case True
                    Case scenario_ = "Filter_Letter_Tasks"
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)
                    Case scenario_ = "Filter_Letter_Parts"
                        afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
                    Case scenario_ = "T_Create_Job_M"
                        afficheinfo_all_wo(scenario_, Sapi, scenario_, scenario_, SCompagnie)
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)
                    Case scenario_ = "btn_Panel_Entrerdestravails_ServerClick"
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)
                    Case scenario_ = "afficheinfo_Bontravail_emp_Scenario_3"
                        afficheinfo_all_wo(scenario_, Sapi, scenario_, scenario_, SCompagnie)
                        afficherBTemployeetjob(scenario_, Sapi, semp, SCompagnie)
                        afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)
                    Case Else
                        scenario_ = scenario_
                        afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)
                End Select



            Case "EMPALL"
                afficheinfo_all_wo(scenario_, Sapi, "ALL", "ALL", SCompagnie)

            Case "EMPBT", "EMPBT-OFF"
                afficherBTemployeetjob(scenario_, Sapi, semp, SCompagnie)

            Case "BT", "ZZ"
                Select Case True
                    Case scenario_ = "btn_Panel_Entrerdespieces_ServerClick"
                        afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)

                    Case scenario_ = "btn_Panel_Entrerdespieces_ServerClick"
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)

                    Case scenario_ = "btn_Panel_Bontravail_ServerClick"
                        afficheinfo_Bontravail_parts(scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BondeTravail, LettreTravail)

                    Case Else
                        scenario_ = scenario_

                End Select
        End Select


    End Sub
    Sub afficheinfo_Bontravail_jobs(scenario_, Sapi, BonDeTravail, LettreTravail)
        If BonDeTravail = Nothing Then Exit Sub

        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Fidelity_id As String = Nothing,
            clientsweb_id As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, clientsweb_id, Fidelity_id, Query_Langue,
                    emailid, securitypin, semp, SCompagnie)

        Try

            Dim ini_Scenario_ As String

            Try
                Dim Dict_afficheinfo_Bontravail_jobs As New Dictionary(Of String, String) From {
     {"btn_Panel_Bontravail_ServerClick", "BT"},
     {"afficheinfo_Bontravail_emp_Scenario_3", "BT"},
     {"T_Create_Job_M", "ALL"},
     {"btn_Panel_Entrerdestravails_ServerClick", "ALL"},
     {"Filter_Letter_Tasks", "ALL"},
     {"Filter_Letter_Parts", "EXIT"},
     {"KeyEND", "Value3"}
 }

                ini_Scenario_ = GetValueIfKeyExists(scenario_, Dict_afficheinfo_Bontravail_jobs)
                Select Case ini_Scenario_
                    Case "Key not found in dictionary."
                        scenario_ = scenario_
                        afficheinfo_Bontravail_jobs(scenario_, Sapi, BonDeTravail, LettreTravail)
                        Exit Sub
                    Case "EXIT"
                        Exit Sub
                End Select
            Catch ex As Exception
                scenario_ = scenario_
                Exit Sub
            End Try

            Dim list_ As New List(Of String)
            Dim Whereline As String
            Select Case True
                Case ini_Scenario_ = "BT"
                Case ini_Scenario_ = "ALL"


                    ddl_Panel_Entrerdestravails_Step_3.Items.Clear()
                    Dim Get_Data_ As New DataTable
                    Dim n_e As String
                    Dim n_e_2 As String = "Aucun Employe"
                    Get_Data_ = GetData("", "employepunch", " WHERE SCompagnie = '" & SCompagnie & "'", 1, 10000)
                    For Each row As DataRow In Get_Data_.Rows
                        Dim employe As String = row("employe").ToString.Trim
                        Dim nom As String = row("nom").ToString.Trim
                        If nom.Contains("/") Then
                            nom = nom.Replace("/", "")
                        End If
                        n_e = nom & " / " & employe
                        If semp = employe Then

                            n_e_2 = n_e
                        End If
                        ddl_Panel_Entrerdestravails_Step_3.Items.Add(n_e)
                    Next
                    ddl_Panel_Entrerdestravails_Step_3.Text = n_e_2

                    If scenario_ <> "Filter_Letter_Tasks" Then

                        dp_Filter_Letter_Tasks.Items.Clear()
                        dp_Filter_Letter_Tasks.Items.Add("Choisir une Lettre")
                        dp_Filter_Letter_Tasks.Items.Add("Tous les jobs")
                    End If

                Case Else
                    scenario_ = scenario_
                    Exit Sub
            End Select

            Select Case True
                Case ini_Scenario_ = "ALL"
                    Select Case True
                        Case scenario_ <> "Filter_Letter_Tasks"
                            Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "%' ORDER BY FLineNumber ASC"
                        Case scenario_ = "Filter_Letter_Tasks" And LettreTravail = "ALL" And tb_Panel_Entrerdestravails_Description.Text <> Nothing
                            Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "%' AND FDescription LIKE '%" & tb_Panel_Entrerdestravails_Description.Text & "%' ORDER BY FLineNumber ASC"
                        Case scenario_ = "Filter_Letter_Tasks" And LettreTravail = "ALL"
                            Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "%' ORDER BY FLineNumber ASC"
                        Case scenario_ = "Filter_Letter_Tasks" And tb_Panel_Entrerdestravails_Description.Text <> Nothing
                            Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "-" & LettreTravail & "%' AND FDescription LIKE '%" & tb_Panel_Entrerdestravails_Description.Text & "%' ORDER BY FLineNumber ASC"
                        Case scenario_ = "Filter_Letter_Tasks"
                            Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "-" & LettreTravail & "%' ORDER BY FLineNumber ASC"
                    End Select
                Case ini_Scenario_ = "BT"
                    Whereline = " WHERE bondetravailjobs_ID LIKE '%" & BonDeTravail & "-" & LettreTravail & "%' ORDER BY FLineNumber ASC"
            End Select

            Dim Get_Data_2 As DataTable = GetData("afficheinfo_Bontravail_jobs", "bondetravailjobs", Whereline, 1, 10000)


            ViewState("dt") = Get_Data_2
            Select Case True
                Case ini_Scenario_ = "BT"
                    Repeaterlettretravail_Taches.DataSource = Get_Data_2
                    Repeaterlettretravail_Taches.DataBind()

                Case ini_Scenario_ = "ALL"
                    If scenario_ <> "Filter_Letter_Tasks" Then
                        For Each row As DataRow In Get_Data_2.Rows
                            If Not list_.Contains(row("LettreTravail").ToString.Trim) Then
                                list_.Add(row("LettreTravail").ToString.Trim)
                            End If
                        Next
                        For Each item As String In list_
                            dp_Filter_Letter_Tasks.Items.Add(New ListItem(item))
                        Next

                        dp_Filter_Letter_Tasks.Text = "Tous les jobs"
                    End If


                    Repeaterlettretravail_Jobs.DataSource = Get_Data_2
                    Repeaterlettretravail_Jobs.DataBind()

            End Select
        Catch ex As Exception

        End Try
    End Sub
    Sub afficheinfo_all_wo(scenario_, Sapi, Scenario_1, Scenario_2, SCompagnie)

        Select Case Scenario_1
            Case "ALL"
            Case "FLASH"
                Select Case Scenario_2
                    Case "BT"
                    Case "JOBS"
                End Select
        End Select

        Select Case Scenario_1
            Case "ALL", "T_Create_Job_M", "afficheinfo_Bontravail_emp_Scenario_3"
                Try
                    If Sapi = Nothing Then Exit Sub
                    Dim BTEmploye As RequestBT = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of RequestBT)(Sapi)
                    For i As Integer = 0 To BTEmploye.FBT.Count - 1
                        Dim FNotefinal As String = BTEmploye.FBT(i).FNotefinal
                        Dim FClient As String = BTEmploye.FClient
                        Dim success As Boolean = InsertOrUpdateHeaderBondeTravail(BTEmploye.FBT(i).FNoBT, FClient, BTEmploye.FBT(i).FNom, BTEmploye.FBT(i).FAdresse, BTEmploye.FBT(i).FProvince,
                                                 BTEmploye.FBT(i).FCodePostal, BTEmploye.FBT(i).FVille, BTEmploye.FBT(i).FTelephone,
                                               BTEmploye.FBT(i).FDate, BTEmploye.FBT(i).FGtotal, BTEmploye.FBT(i).FUnite, BTEmploye.FBT(i).FMarque,
                                               BTEmploye.FBT(i).FModele, BTEmploye.FBT(i).FAnnee, BTEmploye.FBT(i).FSerie, BTEmploye.FBT(i).FCommis,
                                               BTEmploye.FBT(i).FVendeur, BTEmploye.FBT(i).FEtat, BTEmploye.FBT(i).FNotefinal, SCompagnie)


                        For il As Integer = 0 To BTEmploye.FBT(i).FTravailBT.Count - 1

                            Dim FLineNumber As String = Stga_to_Stgm(il)
                            Dim FUnite As String = Stga_to_Stgm(BTEmploye.FBT(i).FUnite)
                            Dim BondeTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FNoBT)
                            Dim Datedu As String = Stga_to_Stgm(BTEmploye.FBT(i).FDate)
                            Dim Budget As String = Stga_to_Stgm(BTEmploye.FBT(i).FEtat)
                            Dim FLettre As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
                            Dim LettreTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
                            Dim FDescriptiontravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FDescriptiontravail)
                            Dim FEtat As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEtat)
                            Dim employe As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)
                            Dim FEmploye As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)


                            For ili As Integer = 0 To BTEmploye.FBT(i).FTravailBT(il).FitemsBT.Count - 1
                                Dim Thirdchild As TreeNode = New TreeNode

                                Dim FPartnumber As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FPartnumber)
                                Dim FDescription As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FDescription)
                                Dim FQte As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FDescription)

                                Dim success3 As Boolean = InsertOrUpdateBondeTravail(BondeTravail & "-" & LettreTravail & "-" & BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FLineNumber, LettreTravail,
                                                                        BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FLineNumber, BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FPartnumber,
                                                                        BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FDescription, LettreTravail,
                                                                        BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FQte, BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FPrice,
                                                                        BTEmploye.FBT(i).FTravailBT(il).FitemsBT(ili).FMontant, "",
                                                                        "", FEtat, FUnite, FClient, employe, SCompagnie)


                            Next



                            Dim success2 As Boolean = InsertOrUpdateBondeTravailJobs(BondeTravail, LettreTravail, FLineNumber, FEtat, FDescriptiontravail, employe, Datedu, Budget, FUnite, FClient, FNotefinal, SCompagnie)



                        Next
                    Next

                Catch ex As Exception
                    'Sresult1 = ex.Message
                End Try
        End Select


        Select Case Scenario_1
            Case "ALL", "FLASH"

                Select Case Scenario_2
                    Case "ALL"
                        Try
                            Dim tb_Search As String
                            Select Case True

                                Case scenario_ = "btn_Panel_Repeater_Metronic_allWO_reset_ServerClick"
                                    tb_Panel_wo_complet.Text = ""
                                    tb_Search = tb_Panel_wo_complet.Text
                                Case scenario_ = "btn_offline_panel_AllJobs_ServerClick" Or scenario_ = "afficheinfo_Bontravail_emp_Scenario_2"
                                    tb_Search = tb_Panel_wo_complet.Text
                                Case scenario_ = "btn_BT_en_cours_employe_complet_ServerClick1" Or scenario_ = "btn_Panel_TreeView_kt_modal_allWO_ServerClick"
                                    tb_Search = tb_Panel_TreeView_kt_modal_allWO.Text

                                Case Else
                                    scenario_ = scenario_
                            End Select
                            Dim Get_Data_ As New DataTable
                            Get_Data_.Columns.Add("FNoBT", GetType(String))
                            Get_Data_.Columns.Add("FUnite", GetType(String))
                            Get_Data_.Columns.Add("FEtat", GetType(String))
                            Get_Data_.Columns.Add("FSerie", GetType(String))
                            Get_Data_.Columns.Add("FNotefinal", GetType(String))
                            Get_Data_.Columns.Add("FNom", GetType(String))
                            Get_Data_.Columns.Add("Jobs", GetType(String))
                            Get_Data_.Columns.Add("Delaie", GetType(String))

                            Dim Get_Data_2 As DataTable
                            Select Case True
                                Case tb_Search = Nothing
                                    Get_Data_2 = GetData("afficheinfo_all_wo", "fbt", " WHERE SCompagnie = '" & SCompagnie & "'", 1, 10000)

                                Case Else
                                    Get_Data_2 = GetData("afficheinfo_all_wo", "fbt", " WHERE FNoBT LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FNom LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FAdresse LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FCodePostal LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FVille LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FTelephone LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FUnite LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FMarque LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FModele LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FAnnee LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FSerie LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FCommis LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FVendeur LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                         "FEtat LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "'", 1, 10000)

                            End Select

                            For Each row As DataRow In Get_Data_2.Rows
                                Dim newRow As DataRow = Get_Data_.NewRow()
                                Dim BondeTravail As String = row("FNoBT").ToString.Trim
                                Dim FUnite As String = row("FUnite").ToString.Trim
                                Dim FMarque As String = row("FMarque").ToString.Trim
                                Dim FModele As String = row("FModele").ToString.Trim
                                Dim FAnnee As String = row("FAnnee").ToString.Trim
                                Dim FSerie As String = row("FSerie").ToString.Trim
                                Dim FNom As String = row("FNom").ToString.Trim
                                Dim FNotefinal As String = row("FNotefinal").ToString.Trim

                                Dim dateCreation As String = row("FDate").ToString.Trim

                                Select Case True
                                    Case dateCreation = Nothing
                                        dateCreation = DateTime.Now().ToString
                                End Select
                                Dim difference As Integer = ComparerDates(dateCreation)
                                Dim Delaie As String = difference.ToString


                                If FNotefinal <> Nothing Then
                                    FNotefinal = FNotefinal
                                End If

                                Dim FDescription As String = Nothing
                                Dim FEtat As String = Nothing
                                Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")

                                Select Case True
                                    Case FNotefinal = "O"
                                        FNotefinal = "text-danger"
                                    Case Else
                                        FNotefinal = "text-success"
                                End Select

                                newRow("FNotefinal") = FNotefinal
                                newRow("FNoBT") = BondeTravail
                                newRow("FUnite") = FAnnee & " / " & FMarque & " / " & FModele
                                newRow("FEtat") = row("FEtat")
                                newRow("Jobs") = ProductToShow_.ToString
                                newRow("FNom") = row("FNom") & GetColumnValueFromDB("fbt", "FNoBT", BondeTravail, "FEtat", SCompagnie)
                                newRow("FSerie") = FSerie & " - " & FUnite
                                newRow("Delaie") = Delaie
                                Get_Data_.Rows.Add(newRow)
                            Next

                            If Get_Data_.Rows.Count > 0 Then
                                ViewState("dt") = Get_Data_

                                Select Case True

                                    Case scenario_ = "btn_BT_en_cours_employe_complet_ServerClick1" Or
                                        scenario_ = "btn_Panel_TreeView_kt_modal_allWO_ServerClick" Or
                                        scenario_ = "afficheinfo_Bontravail_emp_Scenario_2"
                                        Repeater_Metronic_allWO.DataSource = Get_Data_
                                        Repeater_Metronic_allWO.DataBind()
                                    Case scenario_ = "btn_offline_panel_AllJobs_ServerClick"
                                        Panel_Repeater_wo_complet.Visible = True
                                        Repeater_wo_complet.DataSource = Get_Data_
                                        Repeater_wo_complet.DataBind()
                                    Case Else
                                        scenario_ = scenario_
                                End Select


                            End If

                        Catch ex As Exception

                        End Try

                    Case "BT"
                        Try
                            Dim Get_Data_ As New DataTable
                            Get_Data_.Columns.Add("bondetravailjobs_ID", GetType(String))
                            Get_Data_.Columns.Add("FUnite", GetType(String))
                            Get_Data_.Columns.Add("FEtat", GetType(String))
                            Get_Data_.Columns.Add("FSerie", GetType(String))

                            Dim Get_Data_2 As DataTable = GetData("afficheinfo_all_wo", "fbt", "", 1, 10000)

                            For Each row As DataRow In Get_Data_2.Rows
                                Dim newRow As DataRow = Get_Data_.NewRow()
                                Dim BondeTravail As String = row("FNoBT")
                                Dim FUnite As String = row("FUnite")
                                Dim FMarque As String = row("FMarque")
                                Dim FModele As String = row("FModele")
                                Dim FAnnee As String = row("FAnnee")
                                Dim FSerie As String = row("FSerie")
                                Dim FDescription As String = Nothing
                                Dim FEtat As String = Nothing

                                newRow("bondetravailjobs_ID") = BondeTravail
                                newRow("FUnite") = FAnnee & " / " & FMarque & " / " & FModele
                                newRow("FEtat") = row("FEtat")
                                newRow("FSerie") = FSerie & " - " & FUnite
                                Get_Data_.Rows.Add(newRow)
                            Next

                            If Get_Data_.Rows.Count > 0 Then
                                ViewState("dt") = Get_Data_

                                Select Case scenario_
                                    Case "btn_offline_panel_AllJobs_ServerClick"
                                        Repeater_wo_complet.DataSource = Get_Data_
                                        Repeater_wo_complet.DataBind()
                                End Select
                                Repeater_Metronic_allWO.DataSource = Get_Data_
                                Repeater_Metronic_allWO.DataBind()

                            End If

                        Catch ex As Exception

                        End Try
                End Select
        End Select
    End Sub

    Sub afficherBTemployeetjob(scenario_, Sapi, semp, SCompagnie)
        Dim BTEmploye As RequestBT = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of RequestBT)(Sapi)
        semp = semp.ToString.Trim
        Panel_Repeater_Metronic_Btemploye_M_Jobs.Visible = False
        Panel_Repeater_Metronic_Btemploye_M.Visible = True
        Try
            Try
                For i As Integer = 0 To BTEmploye.FBT.Count - 1
                    Dim firstchild As TreeNode = New TreeNode
                    Dim FNoBT As String = Stga_to_Stgm(BTEmploye.FBT(i).FNoBT)
                    Dim FNotefinal As String = Stga_to_Stgm(BTEmploye.FBT(i).FNotefinal)
                    Dim FClient As String = Stga_to_Stgm(BTEmploye.FClient)
                    Dim FNom As String = Stga_to_Stgm(BTEmploye.FBT(i).FNom)
                    Dim FTelephone As String = Stga_to_Stgm(BTEmploye.FBT(i).FTelephone)
                    Dim FGtotal As String = Stga_to_Stgm(BTEmploye.FBT(i).FGtotal)
                    Dim FDate As String = Stga_to_Stgm(BTEmploye.FBT(i).FDate)

                    For il As Integer = 0 To BTEmploye.FBT(i).FTravailBT.Count - 1
                        Dim FLineNumber As String = Stga_to_Stgm(il)
                        Dim BondeTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FNoBT)
                        Dim FUnite As String = Stga_to_Stgm(BTEmploye.FBT(i).FUnite)
                        Dim Datedu As String = Stga_to_Stgm(BTEmploye.FBT(i).FDate)
                        Dim Budget As String = Stga_to_Stgm(BTEmploye.FBT(i).FEtat)
                        Dim FLettre As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
                        Dim LettreTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
                        Dim FDescriptiontravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FDescriptiontravail)
                        Dim FEtat As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEtat)
                        Dim employe As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)
                        Dim FEmploye As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)
                        Dim success2 As Boolean = InsertOrUpdateBondeTravailJobs(BondeTravail, LettreTravail, FLineNumber, FEtat, FDescriptiontravail, employe, Datedu, Budget, FUnite, FClient, FNotefinal, SCompagnie)
                    Next
                Next

            Catch ex As Exception

            End Try
            Try


                'Dim Get_Data_ As New DataTable
                'Get_Data_.Columns.Add("BondeTravail", GetType(String))
                'Get_Data_.Columns.Add("LettreTravail", GetType(String))
                'Get_Data_.Columns.Add("FLineNumber", GetType(String))
                'Get_Data_.Columns.Add("FNotefinal", GetType(String))
                'Get_Data_.Columns.Add("Datedu", GetType(String))
                'Get_Data_.Columns.Add("FEtat", GetType(String))
                'Get_Data_.Columns.Add("Budget", GetType(String))
                'Get_Data_.Columns.Add("FDescription", GetType(String))

                Dim Get_Data_ As New DataTable
                Get_Data_.Columns.Add("FNoBT", GetType(String))
                Get_Data_.Columns.Add("FUnite", GetType(String))
                Get_Data_.Columns.Add("FEtat", GetType(String))
                Get_Data_.Columns.Add("FSerie", GetType(String))
                Get_Data_.Columns.Add("FNotefinal", GetType(String))
                Get_Data_.Columns.Add("FNom", GetType(String))
                Get_Data_.Columns.Add("Jobs", GetType(String))
                Get_Data_.Columns.Add("Delaie", GetType(String))
                Dim Get_Data_2 As DataTable = GetData("afficherBTemployeetjob", "bondetravailjobs", " WHERE employe LIKE '%" & semp & "%' ORDER BY bondetravailjobs_ID ASC", 1, 10000)
                If Get_Data_2.Rows.Count > 0 Then
                    ' Déclarer la liste
                    Dim maListe As New List(Of String)

                    For Each row As DataRow In Get_Data_2.Rows
                        Dim FNoBT As String = row("BondeTravail").ToString.Trim

                        ' Variable à ajouter
                        Dim nouvelleValeur As String = FNoBT
                        ' Vérifier si la liste contient déjà la valeur avant de l'ajouter
                        If Not maListe.Contains(nouvelleValeur) Then
                            maListe.Add(nouvelleValeur)
                            Dim Get_Data_3 As DataTable
                            Dim tb_Search As String = TextBox_Panel_Repeater_Metronic_Btemploye_M.Text
                            Select Case True
                                Case tb_Search = Nothing
                                    Get_Data_3 = GetData("afficheinfo_all_wo", "fbt", " WHERE FNoBT LIKE '%" & FNoBT & "%' AND SCompagnie = '" & SCompagnie & "'", 1, 10000)

                                Case Else
                                    Get_Data_3 = GetData("afficheinfo_all_wo", "fbt", " WHERE FNom LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FNoBT = '" & FNoBT & "' OR " &
                                                                 "FAdresse LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FCodePostal LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FVille LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FTelephone LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FUnite LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FMarque LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FModele LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FAnnee LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FSerie LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FCommis LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FVendeur LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' OR " &
                                                                 "FEtat LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "'", 1, 10000)


                            End Select
                            For Each row2 As DataRow In Get_Data_3.Rows
                                Dim newRow As DataRow = Get_Data_.NewRow()
                                Dim BondeTravail As String = row2("FNoBT").ToString.Trim
                                Dim FUnite As String = row2("FUnite").ToString.Trim
                                Dim FMarque As String = row2("FMarque").ToString.Trim
                                Dim FModele As String = row2("FModele").ToString.Trim
                                Dim FAnnee As String = row2("FAnnee").ToString.Trim
                                Dim FSerie As String = row2("FSerie").ToString.Trim
                                Dim FNom As String = row2("FNom").ToString.Trim
                                Dim FNotefinal As String = row2("FNotefinal").ToString.Trim

                                Dim dateCreation As String = row2("FDate").ToString.Trim

                                Select Case True
                                    Case dateCreation = Nothing
                                        dateCreation = DateTime.Now().ToString
                                End Select
                                Dim difference As Integer = ComparerDates(dateCreation)
                                Dim Delaie As String = difference.ToString


                                If FNotefinal <> Nothing Then
                                    FNotefinal = FNotefinal
                                End If

                                Dim FDescription As String = Nothing
                                Dim FEtat As String = Nothing
                                Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "' AND employe LIKE '%" & semp & "%'")

                                Select Case True
                                    Case FNotefinal = "O"
                                        FNotefinal = "text-danger"
                                    Case Else
                                        FNotefinal = "text-success"
                                End Select

                                newRow("FNotefinal") = FNotefinal
                                newRow("FNoBT") = BondeTravail
                                newRow("FUnite") = FAnnee & " / " & FMarque & " / " & FModele
                                newRow("FEtat") = row2("FEtat")
                                newRow("Jobs") = ProductToShow_.ToString
                                newRow("FNom") = row2("FNom") & GetColumnValueFromDB("fbt", "FNoBT", BondeTravail, "FEtat", SCompagnie)
                                newRow("FSerie") = FSerie & " - " & FUnite
                                newRow("Delaie") = Delaie
                                Get_Data_.Rows.Add(newRow)

                            Next

                        Else
                            Console.WriteLine("La valeur existe déjà dans la liste.")
                        End If
                    Next



                    ViewState("dt") = Get_Data_
                    Repeater_Btemploye_M.DataSource = Get_Data_
                    Repeater_Btemploye_M.DataBind()

                End If
            Catch ex As Exception

            End Try

        Catch ex As Exception
            'Sresult1 = ex.Message
        End Try
    End Sub
    'Sub afficherBTemployeetjobbackup(scenario_, Sapi, semp, SCompagnie)
    '    Dim BTEmploye As RequestBT = Global.Newtonsoft.Json.JsonConvert.DeserializeObject(Of RequestBT)(Sapi)
    '    semp = semp.ToString.Trim
    '    Try
    '        Try
    '            For i As Integer = 0 To BTEmploye.FBT.Count - 1
    '                Dim firstchild As TreeNode = New TreeNode
    '                Dim FNoBT As String = Stga_to_Stgm(BTEmploye.FBT(i).FNoBT)
    '                Dim FNotefinal As String = Stga_to_Stgm(BTEmploye.FBT(i).FNotefinal)
    '                Dim FClient As String = Stga_to_Stgm(BTEmploye.FClient)
    '                Dim FNom As String = Stga_to_Stgm(BTEmploye.FBT(i).FNom)
    '                Dim FTelephone As String = Stga_to_Stgm(BTEmploye.FBT(i).FTelephone)
    '                Dim FGtotal As String = Stga_to_Stgm(BTEmploye.FBT(i).FGtotal)
    '                Dim FDate As String = Stga_to_Stgm(BTEmploye.FBT(i).FDate)

    '                For il As Integer = 0 To BTEmploye.FBT(i).FTravailBT.Count - 1
    '                    Dim FLineNumber As String = Stga_to_Stgm(il)
    '                    Dim BondeTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FNoBT)
    '                    Dim FUnite As String = Stga_to_Stgm(BTEmploye.FBT(i).FUnite)
    '                    Dim Datedu As String = Stga_to_Stgm(BTEmploye.FBT(i).FDate)
    '                    Dim Budget As String = Stga_to_Stgm(BTEmploye.FBT(i).FEtat)
    '                    Dim FLettre As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
    '                    Dim LettreTravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FLettre)
    '                    Dim FDescriptiontravail As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FDescriptiontravail)
    '                    Dim FEtat As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEtat)
    '                    Dim employe As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)
    '                    Dim FEmploye As String = Stga_to_Stgm(BTEmploye.FBT(i).FTravailBT(il).FEmploye)
    '                    Dim success2 As Boolean = InsertOrUpdateBondeTravailJobs(BondeTravail, LettreTravail, FLineNumber, FEtat, FDescriptiontravail, employe, Datedu, Budget, FUnite, FClient, FNotefinal, SCompagnie)
    '                Next
    '            Next

    '        Catch ex As Exception

    '        End Try
    '        Try


    '            Dim Get_Data_ As New DataTable
    '            Get_Data_.Columns.Add("BondeTravail", GetType(String))
    '            Get_Data_.Columns.Add("LettreTravail", GetType(String))
    '            Get_Data_.Columns.Add("FLineNumber", GetType(String))
    '            Get_Data_.Columns.Add("FNotefinal", GetType(String))
    '            Get_Data_.Columns.Add("Datedu", GetType(String))
    '            Get_Data_.Columns.Add("FEtat", GetType(String))
    '            Get_Data_.Columns.Add("Budget", GetType(String))
    '            Get_Data_.Columns.Add("FDescription", GetType(String))


    '            Dim Get_Data_2 As DataTable = GetData("afficherBTemployeetjob", "bondetravailjobs", " WHERE employe LIKE '%" & semp & "%' ORDER BY bondetravailjobs_ID ASC", 1, 10000)
    '            If Get_Data_2.Rows.Count > 0 Then
    '                For Each row As DataRow In Get_Data_2.Rows
    '                    Dim newRow As DataRow = Get_Data_.NewRow()
    '                    Dim BondeTravail As String = row("BondeTravail").ToString.Trim
    '                    Dim LettreTravail As String = row("LettreTravail").ToString.Trim
    '                    Dim FLineNumber As String = row("FLineNumber").ToString.Trim
    '                    Dim FEtat As String = row("FEtat").ToString.Trim
    '                    Dim cssclass As String = row("cssclass").ToString.Trim
    '                    Dim FDescription As String = row("FDescription").ToString.Trim
    '                    Dim employe As String = row("employe").ToString.Trim
    '                    Dim Datedu As String = row("Datedu").ToString.Trim
    '                    Dim Budget As String = row("Budget").ToString.Trim
    '                    Dim FUnite As String = row("FUnite").ToString.Trim
    '                    Dim FClient As String = row("FClient").ToString.Trim
    '                    Dim FNotefinal As String = row("FNotefinal").ToString.Trim

    '                    Dim dateCreation As String = Datedu
    '                    Dim difference As Integer = ComparerDates(dateCreation)
    '                    Dim Delaie As String = difference.ToString


    '                    If Datedu <> Nothing Then
    '                        Datedu = Datedu.Replace("00:00:00", "")
    '                    End If
    '                    If FNotefinal <> Nothing Then
    '                        FNotefinal = FNotefinal
    '                    End If

    '                    Dim nombre_jobs_bt As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")

    '                    Select Case True
    '                        Case FNotefinal = "O"
    '                            FNotefinal = "text-danger"
    '                        Case Else
    '                            FNotefinal = "text-success"
    '                    End Select

    '                    newRow("BondeTravail") = BondeTravail
    '                    newRow("LettreTravail") = LettreTravail
    '                    newRow("FLineNumber") = FLineNumber
    '                    newRow("FNotefinal") = FNotefinal
    '                    newRow("Datedu") = Datedu
    '                    newRow("FEtat") = FEtat
    '                    newRow("Budget") = Budget
    '                    newRow("FDescription") = FDescription
    '                    Get_Data_.Rows.Add(newRow)
    '                Next

    '                ViewState("dt") = Get_Data_

    '                Select Case True

    '                    Case scenario_ = "btn_offline_panel_Jobs_ServerClick" Or scenario_ = "afficheinfo_Bontravail_emp_Scenario_1"
    '                        Repeater_Btemploye.DataSource = Get_Data_
    '                        Repeater_Btemploye.DataBind()


    '                    Case scenario_ = "afficheinfo_Bontravail_emp_Scenario_2" Or scenario_ = "btn_wo_actuel_ServerClick"
    '                        Repeater_Metronic_Btemploye_M.DataSource = Get_Data_
    '                        Repeater_Metronic_Btemploye_M.DataBind()



    '                        Repeater_Metronic_Btemploye_M_Mobile.DataSource = Get_Data_
    '                        Repeater_Metronic_Btemploye_M_Mobile.DataBind()


    '                        Repeater_Btemploye_M.DataSource = Get_Data_
    '                        Repeater_Btemploye_M.DataBind()
    '                    Case Else
    '                        scenario_ = scenario_
    '                End Select

    '            End If
    '        Catch ex As Exception

    '        End Try

    '    Catch ex As Exception
    '        'Sresult1 = ex.Message
    '    End Try
    'End Sub
    Function UrlEncode(input As String) As String
        Dim encoded As New System.Text.StringBuilder()
        For Each ch As Char In input
            Select Case ch
                Case " "c
                    encoded.Append("%20")
                Case "!"c
                    encoded.Append("%21")
                Case """"c
                    encoded.Append("%22")
                Case "#"c
                    encoded.Append("%23")
                Case "$"c
                    encoded.Append("%24")
                Case "%"c
                    encoded.Append("%25")
                Case "&"c
                    encoded.Append("%26")
                Case "'"c
                    encoded.Append("%27")
                Case "("c
                    encoded.Append("%28")
                Case ")"c
                    encoded.Append("%29")
                Case "*"c
                    encoded.Append("%2A")
                Case "+"c
                    encoded.Append("%2B")
                Case ","c
                    encoded.Append("%2C")
                Case "-"c
                    encoded.Append("%2D")
                Case "."c
                    encoded.Append("%2E")
                Case "/"c
                    encoded.Append("%2F")
                Case ":"c
                    encoded.Append("%3A")
                Case ";"c
                    encoded.Append("%3B")
                Case "<"c
                    encoded.Append("%3C")
                Case "="c
                    encoded.Append("%3D")
                Case ">"c
                    encoded.Append("%3E")
                Case "?"c
                    encoded.Append("%3F")
                Case "@"c
                    encoded.Append("%40")
                Case "["c
                    encoded.Append("%5B")
                Case "\"c
                    encoded.Append("%5C")
                Case "]"c
                    encoded.Append("%5D")
                Case "^"c
                    encoded.Append("%5E")
                Case "_"c
                    encoded.Append("%5F")
                Case "`"c
                    encoded.Append("%60")
                Case "{"c
                    encoded.Append("%7B")
                Case "|"c
                    encoded.Append("%7C")
                Case "}"c
                    encoded.Append("%7D")
                Case "~"c
                    encoded.Append("%7E")
                Case Else
                    encoded.Append(ch)
            End Select
        Next
        Return encoded.ToString()
    End Function
    Private Function validerpiece(Scode As String) As Boolean
        Dim bvaliderpiece = False
        Dim Sbt = Lblbt.InnerText.Trim
        Dim Slettre As String = ""
        Dim Ires As Integer = InStr(Sbt, "-")
        If Ires <> 0 Then
            Slettre = Mid(Sbt, Ires + 1)
            Sbt = Mid(Sbt, 1, Ires - 1)
        End If

        Dim valeurAEnvoyer As String = Slienpath & Scode.Trim.PadRight(30) & Add_Parts_qty.Value.Trim.PadRight(8) & Sbt.Trim.PadLeft(10) & Slettre
        Dim valeurEncodee As String = HttpUtility.UrlEncode(valeurAEnvoyer)
        slienemploye = slienip.Trim & "Getretourneinfopiecepunch/resource?id="
        Try

            Dim url As String = slienemploye & valeurEncodee
            ' Créer la requête
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"

            ' Optionnel : Ajouter des en-têtes ou d'autres configurations à la requête
            ' request.Headers.Add("Authorization", "Bearer YOUR_TOKEN")
            Dim result As String
            Try
                ' Envoyer la requête et obtenir la réponse
                Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                    Using reader As New StreamReader(response.GetResponseStream())
                        result = reader.ReadToEnd()
                        ' Traiter le résultat de la réponse
                        Console.WriteLine(result)
                    End Using
                End Using
            Catch ex As WebException
                ' Gérer les erreurs
                Console.WriteLine("Erreur lors de l'appel API: " & ex.Message)
            End Try

            If result = """Reussi""" Then
                bvaliderpiece = True
                Add_Parts_qty.Value = ""
                Add_Parts_id.Value = ""
            End If

        Catch ex As Exception
            Dim Sresult1 As String = ex.Message
        End Try
        '  End If
        Return bvaliderpiece
    End Function
    Sub PunchINOUT()
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        punch_A("*TE", SCompagnie)
    End Sub
    Protected Sub Getretourneclientfixe_(Lblclient_, scenario_, SCompagnie)
        Try
            Lblclient_ = Lblclient_.ToString.Replace("Client: ", "")
            Try

                Search_And_Create_Clients(scenario_, Lblclient_, SCompagnie)
                ' Lier la liste au Repeater
                If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
                    Repeater_Panel_details_cli_detail.DataSource = myList
                    Repeater_Panel_details_cli_detail.DataBind()
                End If

            Catch ex As Exception
                Dim Sresult1 As String = ex.Message
            End Try


        Catch ex As Exception

        End Try

        If scenario_ <> "btn_Panel_PreFacture_ServerClick" Then
            Dim tb_Search As String = tb_Panel_details_veh_detail_historique_jobs.Text

            Try

                Dim Get_Data_2 As DataTable
                Dim FUnite As String = Lblunite_unite.InnerText
                Select Case True
                    Case tb_Search = Nothing
                        Get_Data_2 = GetData("Getretourneclientfixe_", "bondetravailjobs", " WHERE SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "'", 1, 10000)

                    Case Else
                        Get_Data_2 = GetData("Getretourneclientfixe_", "bondetravailjobs", " WHERE FEtat LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "' OR " &
                                                   "FDescription LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "'", 1, 10000)

                End Select

                If Get_Data_2.Rows.Count > 0 Then
                    ViewState("dt") = Get_Data_2
                    Repeater_Panel_details_cli_detail_Histoire_Jobs.DataSource = Get_Data_2
                    Repeater_Panel_details_cli_detail_Histoire_Jobs.DataBind()
                Else
                    Repeater_Panel_details_cli_detail_Histoire_Jobs.Visible = False
                End If

            Catch ex As Exception

            End Try

            Try
                Dim Get_Data_2 As DataTable
                Dim FUnite As String = Lblunite_unite.InnerText
                Select Case True
                    Case tb_Search = Nothing
                        Get_Data_2 = GetData("Getretourneclientfixe_", "bondetravailpiece", " WHERE SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "'", 1, 10000)

                    Case Else
                        Get_Data_2 = GetData("Getretourneclientfixe_", "bondetravailpiece", " WHERE FPartnumber LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "' OR " &
                                                 "BondeTravail LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "' OR " &
                                                 "FDescription LIKE '%" & tb_Search & "%' AND SCompagnie = '" & SCompagnie & "' AND FClient = '" & Lblclient_ & "'", 1, 10000)

                End Select

                If Get_Data_2.Rows.Count > 0 Then
                    ViewState("dt") = Get_Data_2
                    Repeater_Panel_details_cli_detail_Histoire_Parts.DataSource = Get_Data_2
                    Repeater_Panel_details_cli_detail_Histoire_Parts.DataBind()
                Else
                    Repeater_Panel_details_cli_detail_Histoire_Parts.Visible = False
                End If

            Catch ex As Exception

            End Try
        End If

    End Sub

    Sub Load_Bon_Travail_Info(ByRef BondeTravail, ByRef LettreTravail)
        Try
            Select Case True
                Case BondeTravail = Nothing
                Case LettreTravail = "ALL"
                Case BondeTravail = "-"
                    BondeTravail = Nothing
                    LettreTravail = Nothing
                Case Else
                    LettreTravail = BondeTravail.ToString.Substring(BondeTravail.ToString.Length - 1, 1)
                    BondeTravail = BondeTravail.ToString.Substring(0, BondeTravail.ToString.Length - 2)
            End Select
        Catch ex As Exception

        End Try


    End Sub
#End Region

#Region "FORM CLIENT SAVE"





    Async Function Send_Mail(ID_, SHOW_, Entry_dd_id, Clients_id, Fidelity_id, Entry_dd_type_id, Clients_telcel, Type_email, tb_Titre, Textarea1a, Textarea1b, Textarea1c, Textarea1d, Textarea1e, Textarea1f) As Task
        Dim Scenario_ As String = Nothing, Clients_comp As String = Nothing, Clients_contact As String = Nothing, Clients_groupe As String = Nothing, Clients_famille As String = Nothing, Clients_type As String = Nothing, Clients_category As String = Nothing, Clients_addr_1 As String = Nothing, Clients_admin As String = Nothing,
                   Clients_cp_1 As String = Nothing, Clients_ville_1 As String = Nothing, Clients_prov_1 As String = Nothing, Clients_phone_contact As String = Nothing, Clients_fax As String = Nothing, Clients_birthday As String = Nothing, Clients_genre As String = Nothing, Clients_sexe As String = Nothing,
                   Clients_autrea As String = Nothing, Clients_territoire As String = Nothing, Clients_email As String = Nothing

        FindClientInfo(connecStr, Clients_id, Clients_comp, Clients_contact, Clients_groupe, Clients_famille, Clients_type, Clients_category, Fidelity_id, Clients_birthday, Clients_admin, Clients_addr_1, Clients_cp_1, Clients_ville_1,
                            Clients_prov_1, "", Clients_phone_contact, Clients_telcel, Clients_genre, Clients_sexe, Clients_autrea, Clients_territoire, Clients_email)

        Dim txtSubject As String = Generate_txtSubject_Email_FR(Entry_dd_id, Entry_dd_type_id, SHOW_)
        Dim txtMessage_Start As String = "<html><body> "
        Dim txtMessage_End As String = "</body></html> "
        Dim txtMessage As String =
            txtMessage_Start &
         Generate_Header_Email_FR(SHOW_, Entry_dd_type_id) &
        Generate_History_Email_FR(SHOW_, Clients_id, Clients_telcel, Clients_addr_1, Clients_cp_1, Clients_ville_1, Entry_dd_id, tb_Titre, Textarea1a, Textarea1b, Textarea1c, Textarea1d, Textarea1e, Textarea1f) &
              Generate_Footer_Email_FR(SHOW_) &
              txtMessage_End
        Try
            Dim Clients_email_cc As String
            Dim Clients_comp_cc As String = UMan_API_Name

            Select Case True
                Case SHOW_ = "ASSISTANT"
                    Clients_email_cc = EmailComite
                Case SHOW_ = "PROPOSITION"
                    Clients_email_cc = EmailProposition
                Case SHOW_ = "HUMAIN"
                    Clients_email_cc = EmailRecrutement
                Case SHOW_ = "ETUDE"
                    Clients_email_cc = EmailProposition
                Case SHOW_ = "VIDEO"
                    Clients_email_cc = EmailProposition
                Case SHOW_ = "EVENEMENT"
                    Clients_email_cc = EmailEvenement
                Case SHOW_ = "MEDIATHEQUE"
                    Clients_email_cc = EmailProposition
                Case Else
                    EmailMembre = EmailMembre
                    Clients_email_cc = EmailMembre
            End Select

            Await SendEmailAsync("CC", Clients_email, txtSubject, txtMessage, Clients_comp, Clients_email_cc, Clients_comp_cc, "")


        Catch ex As Exception

        End Try
    End Function


#End Region
#Region "FORM MANAGER SEARCH SAVE"
    Protected Sub btn_settings_general_form_save_Click(sender As Object, e As EventArgs)
        Dim stg_db As String = "calendar"
        Dim c_info As String = asp_settings_general_form_mail_info.Value
        Dim c_ventes As String = asp_settings_general_form_mail_ventes.Value
        Dim c_services As String = asp_settings_general_form_mail_services.Value
        Dim c_financement As String = asp_settings_general_form_mail_financement.Value
        Dim t_info As String = asp_settings_general_form_telephone.Value
        Dim d_heures As String = asp_settings_general_form_delais.Value
        Dim calendar_id As String = "CALENDRIER"
        DefaultDataBase = "nor"

        Where_To_Update(stg_db, "calendar_id", calendar_id, c_info, "c_info", DefaultDataBase)
        Where_To_Update(stg_db, "calendar_id", calendar_id, c_ventes, "c_ventes", DefaultDataBase)
        Where_To_Update(stg_db, "calendar_id", calendar_id, c_services, "c_services", DefaultDataBase)
        Where_To_Update(stg_db, "calendar_id", calendar_id, c_financement, "c_financement", DefaultDataBase)
        Where_To_Update(stg_db, "calendar_id", calendar_id, t_info, "t_info", DefaultDataBase)
        Where_To_Update(stg_db, "calendar_id", calendar_id, d_heures, "d_heures", DefaultDataBase)
    End Sub
    Protected Sub btn_settings_general_form_save_Click1(sender As Object, e As EventArgs)
        ' Assurez-vous que toutes les données entrées sont valides
        ' Par exemple, vérifier si les champs ne sont pas vides

        Dim mailInfo As String = asp_settings_general_form_mail_info.Value
        If mailInfo <> Nothing Then
            If IsEmailValid(mailInfo) = False Then
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", mailInfo, "c_info", DefaultDataBase)
            End If
        End If


        Dim mailVentes As String = asp_settings_general_form_mail_ventes.Value
        If mailVentes <> Nothing Then
            If IsEmailValid(mailVentes) = False Then
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", mailVentes, "c_ventes", DefaultDataBase)
            End If
        End If

        Dim mailServices As String = asp_settings_general_form_mail_services.Value
        If mailServices <> Nothing Then
            If IsEmailValid(mailServices) = False Then
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", mailServices, "c_services", DefaultDataBase)
            End If
        End If

        Dim mailFinancement As String = asp_settings_general_form_mail_financement.Value
        If mailFinancement <> Nothing Then
            If IsEmailValid(mailFinancement) = False Then
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", mailFinancement, "c_financement", DefaultDataBase)
            End If
        End If

        Dim telephone As String = asp_settings_general_form_telephone.Value
        If telephone <> Nothing Then
            If RegexTelephone_TO_TEL(telephone) = Nothing Then
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", telephone, "t_info", DefaultDataBase)
            End If
        End If

        Dim delais As String = asp_settings_general_form_delais.Value
        If delais <> Nothing Then

            If Not IsOnlyDigitsUsingRegex(delais) Then
                ' La chaîne contient d'autres caractères en plus des chiffres
                ' Traitement d'erreur ou message à l'utilisateur
                Exit Sub
            Else
                Where_To_Update("calendar", "calendar_id", "CALENDRIER", delais, "d_heures", DefaultDataBase)
            End If
        End If

    End Sub

    Sub Show_Clients(Clients_id)
        Dim sqlReader As MySqlDataReader
        Try

            Dim sqlQuery As String

            sqlQuery = "SELECT * FROM clients WHERE Clients_id = '" & Clients_id & "'"
            Using sqlConn As New MySqlConnection("server=" & DefaultIPAddress & ";port=" & DefaultIPPort & ";uid=" & DefaultUser & ";pwd=" & DefaultPassword & ";database=" & DefaultDataBase)
                Using sqlComm As New MySqlCommand()
                    With sqlComm
                        .Connection = sqlConn
                        .CommandText = sqlQuery
                        .CommandType = CommandType.Text
                    End With
                    Try
                        sqlConn.Open()
                        sqlReader = sqlComm.ExecuteReader()
                        While sqlReader.Read()

                            Dim Clients_comp As String = sqlReader("Clients_comp").ToString()
                            tb_Clients_comp.Value = Clients_comp

                            Dim Clients_contact As String = sqlReader("Clients_contact").ToString()
                            tb_Clients_contact.Value = Clients_contact

                            Dim Clients_groupe As String = sqlReader("Clients_groupe").ToString()
                            tb_Clients_groupe.Value = Clients_groupe

                            Dim Clients_famille As String = sqlReader("Clients_famille").ToString()
                            tb_Clients_famille.Value = Clients_famille

                            Dim Clients_type As String = sqlReader("Clients_type").ToString()
                            tb_Clients_type.Value = Clients_type

                            Dim Clients_category As String = sqlReader("Clients_category").ToString()
                            tb_Clients_category.Value = Clients_category

                            Dim Fidelity_id As String = sqlReader("Fidelity_id").ToString()
                            tb_Fidelity_id.Value = Fidelity_id

                            Dim Clients_addr_1 As String = sqlReader("Clients_addr_1").ToString()
                            tb_Clients_addr_1.Value = Clients_addr_1

                            Dim Clients_cp_1 As String = sqlReader("Clients_cp_1").ToString()
                            tb_Clients_cp_1.Value = Clients_cp_1

                            Dim Clients_ville_1 As String = sqlReader("Clients_ville_1").ToString()
                            tb_Clients_ville_1.Value = Clients_ville_1

                            Dim Clients_prov_1 As String = sqlReader("Clients_prov_1").ToString()
                            tb_Clients_prov_1.Value = Clients_prov_1

                            Dim Clients_pays_1 As String = sqlReader("Clients_pays_1").ToString()
                            tb_Clients_pays_1.Value = Clients_pays_1

                            Dim Clients_phone_contact As String = sqlReader("Clients_phone_contact").ToString()
                            tb_Clients_phone_contact.Value = Clients_phone_contact

                            Dim Clients_telcel As String = sqlReader("Clients_telcel").ToString()
                            tb_Clients_telcel.Value = Clients_telcel

                            Dim Clients_fax As String = sqlReader("Clients_fax").ToString()
                            tb_Clients_fax.Value = Clients_fax

                            Dim Clients_birthday As String = sqlReader("Clients_birthday").ToString()
                            tb_Clients_birthday.Value = Clients_birthday

                            Dim Clients_autrea As String = sqlReader("Clients_autrea").ToString()
                            tb_Clients_autrea.Value = Clients_autrea

                            Dim Clients_territoire As String = sqlReader("Clients_territoire").ToString()
                            tb_Clients_territoire.Value = Clients_territoire

                            Dim Clients_email As String = sqlReader("Clients_email").ToString()
                            tb_Clients_email.Value = Clients_email

                            Dim Clients_admin As String = sqlReader("Clients_admin").ToString()
                            tb_Clients_admin.Value = Clients_admin

                            Dim Clients_genre As String = sqlReader("Clients_genre").ToString()
                            tb_Clients_genre.Value = Clients_genre

                            Dim Clients_sexe As String = sqlReader("Clients_sexe").ToString()
                            tb_Clients_sexe.Value = Clients_sexe

                            Dim Clients_Plateforme As String = sqlReader("Clients_Plateforme").ToString()
                            tb_Clients_Plateforme.Value = Clients_Plateforme


                        End While
                        sqlReader.Close()
                        sqlReader = Nothing

                        sqlConn.Close()
                    Catch ex As Exception
                        sqlConn.Close()
                    End Try
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Sub Show_Product(Prod_id)
        Dim sqlReader As MySqlDataReader
        Try

            Dim sqlQuery As String

            sqlQuery = "SELECT * FROM produits WHERE Prod_id = '" & Prod_id & "'"
            Using sqlConn As New MySqlConnection("server=" & DefaultIPAddress & ";port=" & DefaultIPPort & ";uid=" & DefaultUser & ";pwd=" & DefaultPassword & ";database=" & "bdm")
                Using sqlComm As New MySqlCommand()
                    With sqlComm
                        .Connection = sqlConn
                        .CommandText = sqlQuery
                        .CommandType = CommandType.Text
                    End With
                    Try
                        sqlConn.Open()
                        sqlReader = sqlComm.ExecuteReader()
                        While sqlReader.Read()
                            Dim Prod_Caracteristiques_id As String = sqlReader("Prod_Caracteristiques_id").ToString()
                            Dim Prod_desc As String = sqlReader("Prod_desc").ToString()
                            Dim Prod_desca As String = sqlReader("Prod_desca").ToString()
                            Dim Prod_groupe_id As String = sqlReader("Prod_groupe_id").ToString()
                            Dim Prod_groupe_id_descr_FR As String = sqlReader("Prod_groupe_id_descr_FR").ToString()
                            Dim Prod_groupe_id_descr_EN As String = sqlReader("Prod_groupe_id_descr_EN").ToString()
                            Dim Prod_type_id As String = sqlReader("Prod_type_id").ToString()
                            Dim Prod_type_id_descr_FR As String = sqlReader("Prod_type_id_descr_FR").ToString()
                            Dim Prod_type_id_descr_EN As String = sqlReader("Prod_type_id_descr_EN").ToString()
                            Dim Prod_famille_id As String = sqlReader("Prod_famille_id").ToString()
                            Dim Prod_famille_id_descr_FR As String = sqlReader("Prod_famille_id_descr_FR").ToString()
                            Dim Prod_famille_id_descr_EN As String = sqlReader("Prod_famille_id_descr_EN").ToString()
                            Dim Prod_categorie_id As String = sqlReader("Prod_categorie_id").ToString()
                            Dim Prod_categorie_id_descr_FR As String = sqlReader("Prod_categorie_id_descr_FR").ToString()
                            Dim Prod_categorie_id_descr_EN As String = sqlReader("Prod_categorie_id_descr_EN").ToString()
                            Dim Prod_promo As String = sqlReader("Prod_promo").ToString()
                            Dim Prod_localisation As String = sqlReader("Prod_localisation").ToString()
                            Dim Prod_fournisseur1 As String = sqlReader("Prod_fournisseur1").ToString()
                            Dim Prod_fournisseur2 As String = sqlReader("Prod_fournisseur2").ToString()
                            Dim Prod_fournisseur3 As String = sqlReader("Prod_fournisseur3").ToString()
                            Dim Prod_fournisseur4 As String = sqlReader("Prod_fournisseur4").ToString()
                            Dim Prod_fournisseur5 As String = sqlReader("Prod_fournisseur5").ToString()
                            Dim addf1 As String = sqlReader("addf1").ToString()
                            Dim addf2 As String = sqlReader("addf2").ToString()
                            Dim addf3 As String = sqlReader("addf3").ToString()
                            Dim addf4 As String = sqlReader("addf4").ToString()
                            Dim addf5 As String = sqlReader("addf5").ToString()
                            Dim addf6 As String = sqlReader("addf6").ToString()
                            Dim addf7 As String = sqlReader("addf7").ToString()
                            Dim addf8 As String = sqlReader("addf8").ToString()
                            Dim addf9 As String = sqlReader("addf9").ToString()
                            Dim addf10 As String = sqlReader("addf10").ToString()
                            Dim adde1 As String = sqlReader("adde1").ToString()
                            Dim adde2 As String = sqlReader("adde2").ToString()
                            Dim adde3 As String = sqlReader("adde3").ToString()
                            Dim adde4 As String = sqlReader("adde4").ToString()
                            Dim adde5 As String = sqlReader("adde5").ToString()
                            Dim adde6 As String = sqlReader("adde6").ToString()
                            Dim adde7 As String = sqlReader("adde7").ToString()
                            Dim adde8 As String = sqlReader("adde8").ToString()
                            Dim adde9 As String = sqlReader("adde9").ToString()
                            Dim adde10 As String = sqlReader("adde10").ToString()
                            Dim Prod_prix_Client As String = sqlReader("Prod_prix_Client").ToString()
                            Dim Prod_prix_1 As String = sqlReader("Prod_prix_1").ToString()
                            Dim Prod_prix_2 As String = sqlReader("Prod_prix_2").ToString()
                            Dim Prod_prix_3 As String = sqlReader("Prod_prix_3").ToString()
                            Dim Prod_prix_4 As String = sqlReader("Prod_prix_4").ToString()
                            Dim Prod_prix_5 As String = sqlReader("Prod_prix_5").ToString()
                            Dim Prod_prix_6 As String = sqlReader("Prod_prix_6").ToString()
                            Dim Prod_prix_7 As String = sqlReader("Prod_prix_7").ToString()
                            Dim Prod_prix_8 As String = sqlReader("Prod_prix_8").ToString()
                            Dim Prod_prix_9 As String = sqlReader("Prod_prix_9").ToString()
                            Dim Prod_prix_10 As String = sqlReader("Prod_prix_10").ToString()
                            Dim Prod_Cout_Courant As String = sqlReader("Prod_Cout_Courant").ToString()
                            Dim Prod_Cout_Fournisseur1 As String = sqlReader("Prod_Cout_Fournisseur1").ToString()
                            Dim Prod_Cout_Fournisseur2 As String = sqlReader("Prod_Cout_Fournisseur2").ToString()
                            Dim Prod_Cout_Fournisseur3 As String = sqlReader("Prod_Cout_Fournisseur3").ToString()
                            Dim Prod_Cout_Fournisseur4 As String = sqlReader("Prod_Cout_Fournisseur4").ToString()
                            Dim Prod_Cout_Fournisseur5 As String = sqlReader("Prod_Cout_Fournisseur5").ToString()
                            Dim Prod_ligne As String = sqlReader("Prod_ligne").ToString()
                            Dim Prod_couleur As String = sqlReader("Prod_couleur").ToString()
                            Dim Prod_grandeur As String = sqlReader("Prod_grandeur").ToString()
                            Dim Prod_longueur As String = sqlReader("Prod_longueur").ToString()
                            Dim Prod_largeur As String = sqlReader("Prod_largeur").ToString()
                            Dim Prod_hauteur As String = sqlReader("Prod_hauteur").ToString()
                            Dim Prod_poids As String = sqlReader("Prod_poids").ToString()
                            Dim Prod_genre As String = sqlReader("Prod_genre").ToString()
                            Dim Prod_style As String = sqlReader("Prod_style").ToString()
                            Dim Prod_Style_slingshot As String = sqlReader("Prod_Style_slingshot").ToString()
                            Dim Prod_Style_scooter As String = sqlReader("Prod_Style_scooter").ToString()
                            Dim Prod_Style_atv As String = sqlReader("Prod_Style_atv").ToString()
                            Dim Prod_Style_snow As String = sqlReader("Prod_Style_snow").ToString()
                            Dim Prod_Style_motorcycle As String = sqlReader("Prod_Style_motorcycle").ToString()
                            Dim Prod_Style_motocross As String = sqlReader("Prod_Style_motocross").ToString()
                            Dim Prod_Style_marine As String = sqlReader("Prod_Style_marine").ToString()
                            Dim Prod_saison As String = sqlReader("Prod_saison").ToString()
                            Dim Prod_qte As String = sqlReader("Prod_qte").ToString()
                            Dim cat As String = sqlReader("cat").ToString()
                            Dim descr As String = sqlReader("descr").ToString()
                            Dim desca As String = sqlReader("desca").ToString()
                            Dim Prix1 As String = sqlReader("Prix1").ToString()
                            Dim Quant As String = sqlReader("Quant").ToString()
                            Dim Qcom As String = sqlReader("Qcom").ToString()
                            Dim cout As String = sqlReader("cout").ToString()
                            Dim liendetail As String = sqlReader("liendetail").ToString()
                            Dim Prod_Marque_Financement As String = sqlReader("Prod_Marque_Financement").ToString()
                            Dim Prod_Modele_Financement As String = sqlReader("Prod_Modele_Financement").ToString()
                            Dim Prod_Groupe_Financement As String = sqlReader("Prod_Groupe_Financement").ToString()
                            Dim Prod_Famille_Financement As String = sqlReader("Prod_Famille_Financement").ToString()
                            Dim Prod_Annee_Financement As String = sqlReader("Prod_Annee_Financement").ToString()
                            Dim Prod_Actif As String = sqlReader("Prod_Actif").ToString()
                            Dim Prod_photo_url1 As String = sqlReader("Prod_photo_url1").ToString()
                            Dim Prod_photo_url2 As String = sqlReader("Prod_photo_url2").ToString()
                            Dim Prod_photo_url3 As String = sqlReader("Prod_photo_url3").ToString()
                            Dim Prod_photo_url4 As String = sqlReader("Prod_photo_url4").ToString()
                            Dim Prod_photo_url5 As String = sqlReader("Prod_photo_url5").ToString()
                            Dim Prod_photo_url6 As String = sqlReader("Prod_photo_url6").ToString()
                            Dim Prod_photo_url7 As String = sqlReader("Prod_photo_url7").ToString()
                            Dim Prod_photo_url8 As String = sqlReader("Prod_photo_url8").ToString()
                            Dim Prod_photo_url9 As String = sqlReader("Prod_photo_url9").ToString()
                            Dim Prod_photo_url10 As String = sqlReader("Prod_photo_url10").ToString()
                            Dim Prod_photo_url11 As String = sqlReader("Prod_photo_url11").ToString()
                            Dim Prod_photo_url12 As String = sqlReader("Prod_photo_url12").ToString()
                            Dim Prod_photo_url13 As String = sqlReader("Prod_photo_url13").ToString()
                            Dim Prod_photo_url14 As String = sqlReader("Prod_photo_url14").ToString()
                            Dim Prod_photo_url15 As String = sqlReader("Prod_photo_url15").ToString()


                            tb_Prod_Caracteristiques_id.Value = Prod_Caracteristiques_id
                            tb_Prod_id.Value = Prod_id
                            tb_Prod_desc.Value = Prod_desc
                            tb_Prod_desca.Value = Prod_desca
                            tb_Prod_groupe_id.Value = Prod_groupe_id
                            tb_Prod_groupe_id_descr_FR.Value = Prod_groupe_id_descr_FR
                            tb_Prod_groupe_id_descr_EN.Value = Prod_groupe_id_descr_EN
                            tb_Prod_type_id.Value = Prod_type_id
                            tb_Prod_type_id_descr_FR.Value = Prod_type_id_descr_FR
                            tb_Prod_type_id_descr_EN.Value = Prod_type_id_descr_EN
                            tb_Prod_famille_id.Value = Prod_famille_id
                            tb_Prod_famille_id_descr_FR.Value = Prod_famille_id_descr_FR
                            tb_Prod_famille_id_descr_EN.Value = Prod_famille_id_descr_EN
                            tb_Prod_categorie_id.Value = Prod_categorie_id
                            tb_Prod_categorie_id_descr_FR.Value = Prod_categorie_id_descr_FR
                            tb_Prod_categorie_id_descr_EN.Value = Prod_categorie_id_descr_EN
                            tb_Prod_promo.Value = Prod_promo
                            tb_Prod_localisation.Value = Prod_localisation
                            tb_Prod_fournisseur1.Value = Prod_fournisseur1
                            tb_Prod_fournisseur2.Value = Prod_fournisseur2
                            tb_Prod_fournisseur3.Value = Prod_fournisseur3
                            tb_Prod_fournisseur4.Value = Prod_fournisseur4
                            tb_Prod_fournisseur5.Value = Prod_fournisseur5
                            tb_addf1.Value = addf1
                            tb_addf2.Value = addf2
                            tb_addf3.Value = addf3
                            tb_addf4.Value = addf4
                            tb_addf5.Value = addf5
                            tb_addf6.Value = addf6
                            tb_addf7.Value = addf7
                            tb_addf8.Value = addf8
                            tb_addf9.Value = addf9
                            tb_addf10.Value = addf10
                            tb_adde1.Value = adde1
                            tb_adde2.Value = adde2
                            tb_adde3.Value = adde3
                            tb_adde4.Value = adde4
                            tb_adde5.Value = adde5
                            tb_adde6.Value = adde6
                            tb_adde7.Value = adde7
                            tb_adde8.Value = adde8
                            tb_adde9.Value = adde9
                            tb_adde10.Value = adde10
                            tb_Prod_prix_Client.Value = Prod_prix_Client
                            tb_Prod_prix_1.Value = Prod_prix_1
                            tb_Prod_prix_2.Value = Prod_prix_2
                            tb_Prod_prix_3.Value = Prod_prix_3
                            tb_Prod_prix_4.Value = Prod_prix_4
                            tb_Prod_prix_5.Value = Prod_prix_5
                            tb_Prod_prix_6.Value = Prod_prix_6
                            tb_Prod_prix_7.Value = Prod_prix_7
                            tb_Prod_prix_8.Value = Prod_prix_8
                            tb_Prod_prix_9.Value = Prod_prix_9
                            tb_Prod_prix_10.Value = Prod_prix_10
                            tb_Prod_Cout_Courant.Value = Prod_Cout_Courant
                            tb_Prod_Cout_Fournisseur1.Value = Prod_Cout_Fournisseur1
                            tb_Prod_Cout_Fournisseur2.Value = Prod_Cout_Fournisseur2
                            tb_Prod_Cout_Fournisseur3.Value = Prod_Cout_Fournisseur3
                            tb_Prod_Cout_Fournisseur4.Value = Prod_Cout_Fournisseur4
                            tb_Prod_Cout_Fournisseur5.Value = Prod_Cout_Fournisseur5
                            tb_Prod_ligne.Value = Prod_ligne
                            tb_Prod_couleur.Value = Prod_couleur
                            tb_Prod_grandeur.Value = Prod_grandeur
                            tb_Prod_longueur.Value = Prod_longueur
                            tb_Prod_largeur.Value = Prod_largeur
                            tb_Prod_hauteur.Value = Prod_hauteur
                            tb_Prod_poids.Value = Prod_poids
                            tb_Prod_genre.Value = Prod_genre
                            tb_Prod_style.Value = Prod_style
                            tb_Prod_Style_slingshot.Value = Prod_Style_slingshot
                            tb_Prod_Style_scooter.Value = Prod_Style_scooter
                            tb_Prod_Style_atv.Value = Prod_Style_atv
                            tb_Prod_Style_snow.Value = Prod_Style_snow
                            tb_Prod_Style_motorcycle.Value = Prod_Style_motorcycle
                            tb_Prod_Style_motocross.Value = Prod_Style_motocross
                            tb_Prod_Style_marine.Value = Prod_Style_marine
                            tb_Prod_saison.Value = Prod_saison
                            tb_Prod_qte.Value = Prod_qte
                            tb_Prod_Marque_Financement.Value = Prod_Marque_Financement
                            tb_Prod_Modele_Financement.Value = Prod_Modele_Financement
                            tb_Prod_Groupe_Financement.Value = Prod_Groupe_Financement
                            tb_Prod_Famille_Financement.Value = Prod_Famille_Financement
                            tb_Prod_Annee_Financement.Value = Prod_Annee_Financement
                            tb_Prod_Actif.Value = Prod_Actif
                            tb_Prod_photo_url1.Value = Prod_photo_url1
                            tb_Prod_photo_url2.Value = Prod_photo_url2
                            tb_Prod_photo_url3.Value = Prod_photo_url3
                            tb_Prod_photo_url4.Value = Prod_photo_url4
                            tb_Prod_photo_url5.Value = Prod_photo_url5
                            tb_Prod_photo_url6.Value = Prod_photo_url6
                            tb_Prod_photo_url7.Value = Prod_photo_url7
                            tb_Prod_photo_url8.Value = Prod_photo_url8
                            tb_Prod_photo_url9.Value = Prod_photo_url9
                            tb_Prod_photo_url10.Value = Prod_photo_url10
                            tb_Prod_photo_url11.Value = Prod_photo_url11
                            tb_Prod_photo_url12.Value = Prod_photo_url12
                            tb_Prod_photo_url13.Value = Prod_photo_url13
                            tb_Prod_photo_url14.Value = Prod_photo_url14
                            tb_Prod_photo_url15.Value = Prod_photo_url15

                        End While
                        sqlReader.Close()
                        sqlReader = Nothing

                        sqlConn.Close()
                    Catch ex As Exception
                        sqlConn.Close()
                    End Try
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Sub Show_Vehicule(Unites_id)
        Dim sqlReader As MySqlDataReader
        Try

            Dim sqlQuery As String
            Dim stg_db As String = "vehicules"

            sqlQuery = "SELECT * FROM " & stg_db & " WHERE Unites_id = '" & Unites_id & "'"
            Dim Connecstr_ As String
            Select Case True
                Case stg_db = "clients"
                    Connecstr_ = "server=" & DefaultIPAddress & ";port=" & DefaultIPPort & ";uid=" & DefaultUser & ";pwd=" & DefaultPassword & ";database=" & DefaultDataBase
                Case Else
                    Connecstr_ = "server=" & DefaultIPAddress & ";port=" & DefaultIPPort & ";uid=" & DefaultUser & ";pwd=" & DefaultPassword & ";database=" & "nor"
            End Select

            Using sqlConn As New MySqlConnection(Connecstr_)
                Using sqlComm As New MySqlCommand()
                    With sqlComm
                        .Connection = sqlConn
                        .CommandText = sqlQuery
                        .CommandType = CommandType.Text
                    End With
                    Try
                        sqlConn.Open()
                        sqlReader = sqlComm.ExecuteReader()
                        While sqlReader.Read()

                            Dim Unites_groupe As String = sqlReader("Unites_groupe").ToString()
                            Dim Unites_groupe_descr_FR As String = sqlReader("Unites_groupe_descr_FR").ToString()
                            Dim Unites_groupe_descr_EN As String = sqlReader("Unites_groupe_descr_EN").ToString()
                            Dim Unites_famille As String = sqlReader("Unites_famille").ToString()
                            Dim Unites_famille_descr_FR As String = sqlReader("Unites_famille_descr_FR").ToString()
                            Dim Unites_famille_descr_EN As String = sqlReader("Unites_famille_descr_EN").ToString()
                            Dim Unites_categorie As String = sqlReader("Unites_categorie").ToString()
                            Dim Unites_type As String = sqlReader("Unites_type").ToString()
                            Dim Unites_groupe_class As String = sqlReader("Unites_groupe_class").ToString()
                            Dim Unites_groupe_class_descr_FR As String = sqlReader("Unites_groupe_class_descr_FR").ToString()
                            Dim Unites_groupe_class_descr_EN As String = sqlReader("Unites_groupe_class_descr_EN").ToString()
                            Dim Unites_marque As String = sqlReader("Unites_marque").ToString()
                            Dim Unites_modele As String = sqlReader("Unites_modele").ToString()
                            Dim Unites_descr As String = sqlReader("Unites_descr").ToString()
                            Dim Unites_description_web As String = sqlReader("Unites_description_web").ToString()
                            Dim Unites_drivetrain As String = sqlReader("Unites_drivetrain").ToString()
                            Dim Unites_transmission As String = sqlReader("Unites_transmission").ToString()
                            Dim Unites_fueltype As String = sqlReader("Unites_fueltype").ToString()
                            Dim Unites_longueur As String = sqlReader("Unites_longueur").ToString()
                            Dim Unites_nbext As String = sqlReader("Unites_nbext").ToString()
                            Dim Unites_nbcou As String = sqlReader("Unites_nbcou").ToString()
                            Dim Unites_poidvid As String = sqlReader("Unites_poidvid").ToString()
                            Dim Unites_poidtimo As String = sqlReader("Unites_poidtimo").ToString()
                            Dim Unites_desactiver As String = sqlReader("Unites_desactiver").ToString()
                            Dim Unites_annee As String = sqlReader("Unites_annee").ToString()
                            Dim Unites_statut As String = sqlReader("Unites_statut").ToString()
                            Dim Unites_client As String = sqlReader("Unites_client").ToString()
                            Dim Unites_interne As String = sqlReader("Unites_interne").ToString()
                            Dim Unites_manufactur As String = sqlReader("Unites_manufactur").ToString()
                            Dim Unites_stock As String = sqlReader("Unites_stock").ToString()
                            Dim Unites_serie As String = sqlReader("Unites_serie").ToString()
                            Dim Unites_ser2 As String = sqlReader("Unites_ser2").ToString()
                            Dim Unites_ser3 As String = sqlReader("Unites_ser3").ToString()
                            Dim Unites_ser4 As String = sqlReader("Unites_ser4").ToString()
                            Dim Unites_couleur As String = sqlReader("Unites_couleur").ToString()
                            Dim Unites_poids As String = sqlReader("Unites_poids").ToString()
                            Dim Unites_cylindre As String = sqlReader("Unites_cylindre").ToString()
                            Dim Unites_nb_passager As String = sqlReader("Unites_nb_passager").ToString()
                            Dim Unites_nb_pneu As String = sqlReader("Unites_nb_pneu").ToString()
                            Dim Unites_moteur As String = sqlReader("Unites_moteur").ToString()
                            Dim Unites_odometre As String = sqlReader("Unites_odometre").ToString()
                            Dim Unites_prix As String = sqlReader("Unites_prix").ToString()
                            Dim Unites_prix_afficher As String = sqlReader("Unites_prix_afficher").ToString()
                            Dim Unites_prix_promo As String = sqlReader("Unites_prix_promo").ToString()
                            Dim Unites_promo As String = sqlReader("Unites_promo").ToString()
                            Dim Unites_frais_manutention As String = sqlReader("Unites_frais_manutention").ToString()
                            Dim Unites_frais_transport As String = sqlReader("Unites_frais_transport").ToString()
                            Dim Unites_Payment_Interest As String = sqlReader("Unites_Payment_Interest").ToString()
                            Dim Unites_Payment_MonthPeriod As String = sqlReader("Unites_Payment_MonthPeriod").ToString()
                            Dim Unites_Payment As String = sqlReader("Unites_Payment").ToString()
                            Dim Unites_rabais As String = sqlReader("Unites_rabais").ToString()
                            Dim Unites_liendetail As String = sqlReader("Unites_liendetail").ToString()
                            Dim Unites_localisation As String = sqlReader("Unites_localisation").ToString()
                            Dim Unites_res9 As String = sqlReader("Unites_res9").ToString()
                            Dim Unites_res10 As String = sqlReader("Unites_res10").ToString()
                            Dim Unites_EnLigne As String = sqlReader("Unites_EnLigne").ToString()
                            Dim boit1 As String = sqlReader("boit1").ToString()
                            Dim boit2 As String = sqlReader("boit2").ToString()
                            Dim boit3 As String = sqlReader("boit3").ToString()
                            Dim boit4 As String = sqlReader("boit4").ToString()
                            Dim boit5 As String = sqlReader("boit5").ToString()
                            Dim boit6 As String = sqlReader("boit6").ToString()
                            Dim boit7 As String = sqlReader("boit7").ToString()
                            Dim boit8 As String = sqlReader("boit8").ToString()
                            Dim boit9 As String = sqlReader("boit9").ToString()
                            Dim boit10 As String = sqlReader("boit10").ToString()
                            Dim aut1 As String = sqlReader("aut1").ToString()
                            Dim aut2 As String = sqlReader("aut2").ToString()
                            Dim aut3 As String = sqlReader("aut3").ToString()
                            Dim aut4 As String = sqlReader("aut4").ToString()
                            Dim aut5 As String = sqlReader("aut5").ToString()
                            Dim aut6 As String = sqlReader("aut6").ToString()
                            Dim aut7 As String = sqlReader("aut7").ToString()
                            Dim aut8 As String = sqlReader("aut8").ToString()
                            Dim aut9 As String = sqlReader("aut9").ToString()
                            Dim aut10 As String = sqlReader("aut10").ToString()
                            Dim Unites_photo_url1 As String = sqlReader("Unites_photo_url1").ToString()
                            Dim Unites_photo_url2 As String = sqlReader("Unites_photo_url2").ToString()
                            Dim Unites_photo_url3 As String = sqlReader("Unites_photo_url3").ToString()
                            Dim Unites_photo_url4 As String = sqlReader("Unites_photo_url4").ToString()
                            Dim Unites_photo_url5 As String = sqlReader("Unites_photo_url5").ToString()
                            Dim Unites_photo_url6 As String = sqlReader("Unites_photo_url6").ToString()
                            Dim Unites_photo_url7 As String = sqlReader("Unites_photo_url7").ToString()
                            Dim Unites_photo_url8 As String = sqlReader("Unites_photo_url8").ToString()
                            Dim Unites_photo_url9 As String = sqlReader("Unites_photo_url9").ToString()
                            Dim Unites_photo_url10 As String = sqlReader("Unites_photo_url10").ToString()
                            Dim Unites_photo_url11 As String = sqlReader("Unites_photo_url11").ToString()
                            Dim Unites_photo_url12 As String = sqlReader("Unites_photo_url12").ToString()
                            Dim Unites_photo_url13 As String = sqlReader("Unites_photo_url13").ToString()
                            Dim Unites_photo_url14 As String = sqlReader("Unites_photo_url14").ToString()
                            Dim Unites_photo_url15 As String = sqlReader("Unites_photo_url15").ToString()


                            tb_Unites_id.Value = Unites_id
                            tb_Unites_groupe.Value = Unites_groupe
                            tb_Unites_groupe_descr_FR.Value = Unites_groupe_descr_FR
                            tb_Unites_groupe_descr_EN.Value = Unites_groupe_descr_EN
                            tb_Unites_famille.Value = Unites_famille
                            tb_Unites_famille_descr_FR.Value = Unites_famille_descr_FR
                            tb_Unites_famille_descr_EN.Value = Unites_famille_descr_EN
                            tb_Unites_categorie.Value = Unites_categorie
                            tb_Unites_type.Value = Unites_type
                            tb_Unites_groupe_class.Value = Unites_groupe_class
                            tb_Unites_groupe_class_descr_FR.Value = Unites_groupe_class_descr_FR
                            tb_Unites_groupe_class_descr_EN.Value = Unites_groupe_class_descr_EN
                            tb_Unites_marque.Value = Unites_marque
                            tb_Unites_modele.Value = Unites_modele
                            tb_Unites_descr.Value = Unites_descr
                            tb_Unites_description_web.Value = Unites_description_web
                            tb_Unites_drivetrain.Value = Unites_drivetrain
                            tb_Unites_transmission.Value = Unites_transmission
                            tb_Unites_fueltype.Value = Unites_fueltype
                            tb_Unites_longueur.Value = Unites_longueur
                            tb_Unites_nbext.Value = Unites_nbext
                            tb_Unites_nbcou.Value = Unites_nbcou
                            tb_Unites_poidvid.Value = Unites_poidvid
                            tb_Unites_poidtimo.Value = Unites_poidtimo
                            tb_Unites_desactiver.Value = Unites_desactiver
                            tb_Unites_annee.Value = Unites_annee
                            tb_Unites_statut.Value = Unites_statut
                            tb_Unites_client.Value = Unites_client
                            tb_Unites_interne.Value = Unites_interne
                            tb_Unites_manufactur.Value = Unites_manufactur
                            tb_Unites_stock.Value = Unites_stock
                            tb_Unites_serie.Value = Unites_serie
                            tb_Unites_ser2.Value = Unites_ser2
                            tb_Unites_ser3.Value = Unites_ser3
                            tb_Unites_ser4.Value = Unites_ser4
                            tb_Unites_couleur.Value = Unites_couleur
                            tb_Unites_poids.Value = Unites_poids
                            tb_Unites_cylindre.Value = Unites_cylindre
                            tb_Unites_nb_passager.Value = Unites_nb_passager
                            tb_Unites_nb_pneu.Value = Unites_nb_pneu
                            tb_Unites_moteur.Value = Unites_moteur
                            tb_Unites_odometre.Value = Unites_odometre
                            tb_Unites_prix.Value = Unites_prix
                            tb_Unites_prix_afficher.Value = Unites_prix_afficher
                            tb_Unites_prix_promo.Value = Unites_prix_promo
                            tb_Unites_promo.Value = Unites_promo
                            tb_Unites_frais_manutention.Value = Unites_frais_manutention
                            tb_Unites_frais_transport.Value = Unites_frais_transport
                            tb_Unites_Payment_Interest.Value = Unites_Payment_Interest
                            tb_Unites_Payment_MonthPeriod.Value = Unites_Payment_MonthPeriod
                            tb_Unites_Payment.Value = Unites_Payment
                            tb_Unites_rabais.Value = Unites_rabais
                            tb_Unites_liendetail.Value = Unites_liendetail
                            tb_Unites_localisation.Value = Unites_localisation
                            tb_Unites_res9.Value = Unites_res9
                            tb_Unites_res10.Value = Unites_res10
                            tb_Unites_EnLigne.Value = Unites_EnLigne
                            tb_boit1.Value = boit1
                            tb_boit2.Value = boit2
                            tb_boit3.Value = boit3
                            tb_boit4.Value = boit4
                            tb_boit5.Value = boit5
                            tb_boit6.Value = boit6
                            tb_boit7.Value = boit7
                            tb_boit8.Value = boit8
                            tb_boit9.Value = boit9
                            tb_boit10.Value = boit10
                            tb_aut1.Value = aut1
                            tb_aut2.Value = aut2
                            tb_aut3.Value = aut3
                            tb_aut4.Value = aut4
                            tb_aut5.Value = aut5
                            tb_aut6.Value = aut6
                            tb_aut7.Value = aut7
                            tb_aut8.Value = aut8
                            tb_aut9.Value = aut9
                            tb_aut10.Value = aut10
                            tb_Unites_photo_url1.Value = Unites_photo_url1
                            tb_Unites_photo_url2.Value = Unites_photo_url2
                            tb_Unites_photo_url3.Value = Unites_photo_url3
                            tb_Unites_photo_url4.Value = Unites_photo_url4
                            tb_Unites_photo_url5.Value = Unites_photo_url5
                            tb_Unites_photo_url6.Value = Unites_photo_url6
                            tb_Unites_photo_url7.Value = Unites_photo_url7
                            tb_Unites_photo_url8.Value = Unites_photo_url8
                            tb_Unites_photo_url9.Value = Unites_photo_url9
                            tb_Unites_photo_url10.Value = Unites_photo_url10
                            tb_Unites_photo_url11.Value = Unites_photo_url11
                            tb_Unites_photo_url12.Value = Unites_photo_url12
                            tb_Unites_photo_url13.Value = Unites_photo_url13
                            tb_Unites_photo_url14.Value = Unites_photo_url14
                            tb_Unites_photo_url15.Value = Unites_photo_url15

                        End While
                        sqlReader.Close()
                        sqlReader = Nothing

                        sqlConn.Close()
                    Catch ex As Exception
                        sqlConn.Close()
                    End Try
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub search_Clients_id_ServerClick(sender As Object, e As EventArgs)
        Show_Clients(tb_Clients_id_search.Value)
    End Sub
    Protected Sub save_Clients_id_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub search_Unites_id_ServerClick(sender As Object, e As EventArgs)
        Show_Vehicule(tb_Unites_id_search.Value)
    End Sub
    Protected Sub save_Unites_id_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub search_Prod_id_ServerClick(sender As Object, e As EventArgs)
        Show_Product(tb_Prod_id_search.Value)
    End Sub
    Protected Sub save_Prod_id_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub save_Fourn_id_ServerClick(sender As Object, e As EventArgs)

    End Sub
    Protected Sub search_Fourn_id_ServerClick(sender As Object, e As EventArgs)

    End Sub
#End Region
#Region "FORM MANAGER LISTE"
    Sub Update_Pagenumber(module_, pageNumber)
        Select Case True
            Case module_ = "Vehicules_Data"
                Vehicules_pageNumber.Value = pageNumber
            Case module_ = "Produits_Data"
                produits_pageNumber.Value = pageNumber
            Case module_ = "Clients_Data"
                clients_pageNumber.Value = pageNumber
            Case module_ = "Fournisseur_Data"
                Fournisseur_pageNumber.Value = pageNumber
            Case module_ = "Montage_Data"
                montage_pageNumber.Value = pageNumber
            Case module_ = "historique"
                Vehicules_pageNumber.Value = pageNumber
            Case module_ = "vehicules"
                Vehicules_pageNumber.Value = pageNumber
        End Select
    End Sub
    Protected Sub filter_div_vehicules_ServerClick(sender As Object, e As EventArgs)
        Vehicules_Data("filter")
    End Sub
    Protected Sub foward_pnl_div_vehicules_ServerClick(sender As Object, e As EventArgs)
        Vehicules_Data("foward")
    End Sub
    Protected Sub back_pnl_div_vehicules_ServerClick(sender As Object, e As EventArgs)
        Vehicules_Data("back")
    End Sub
    Sub Vehicules_Data(scenario)
        Dim module_ As String = "Vehicules_Data"
        Dim pageNumber As String = Vehicules_pageNumber.Value
        Dim recordsPerPage As String = Vehicules_recordsPerPage.Text
        Process_Pager(pageNumber, recordsPerPage, scenario, module_)
        Update_Pagenumber(module_, pageNumber)

        Dim WhereLine As String = Nothing
        Dim Unites_client As String = filter_div_vehicules_Unites_client.Text
        Select Case True
            Case Unites_client = "Status"
            Case Unites_client = "Tous"
            Case Unites_client = "Interne"
                WhereLine = " WHERE Unites_client = 'INTERNE' "
            Case Unites_client = "En financement"
                WhereLine = " WHERE Unites_client = 'INTERNE' and Unites_res9 = 'O' "

        End Select

        Dim Unites_photo_url1 As String = filter_div_vehicules_Unites_photo_url1.Text
        Select Case True
            Case Unites_photo_url1 <> "Sans Photo"
            Case Unites_photo_url1 <> "" And WhereLine = Nothing
                WhereLine = " WHERE Unites_photo_url1 = 'Models/images/A VENIR VEH.jpg' "
            Case Unites_photo_url1 <> "" And WhereLine <> Nothing
                WhereLine += "AND Unites_photo_url1 = 'Models/images/A VENIR VEH.jpg' "
        End Select


        Dim Unites_modele As String = tb_modele.Text
        Select Case True
            Case Unites_modele <> "" And WhereLine = Nothing
                WhereLine = " WHERE Unites_modele LIKE '%" & Unites_modele & "%' "
            Case Unites_modele <> "" And WhereLine <> Nothing
                WhereLine += "AND Unites_modele LIKE '%" & Unites_modele & "%' "
        End Select

        Dim Unites_marque As String = tb_marque.Text
        Select Case True
            Case Unites_marque <> "" And WhereLine = Nothing
                WhereLine = " WHERE Unites_marque LIKE '%" & Unites_marque & "%' "
            Case Unites_marque <> "" And WhereLine <> Nothing
                WhereLine += "AND Unites_marque LIKE '%" & Unites_marque & "%' "
        End Select

        Dim Unites_annee As String = tb_annee.Text
        Select Case True
            Case Unites_annee <> "" And WhereLine = Nothing
                WhereLine = " WHERE Unites_annee LIKE '%" & Unites_annee & "%' "
            Case Unites_annee <> "" And WhereLine <> Nothing
                WhereLine += "AND Unites_annee LIKE '%" & Unites_annee & "%' "
            Case Else
        End Select

        Dim tb_search As String = tb_veh.Text
        Select Case True
            Case tb_search <> "" And WhereLine = Nothing

                WhereLine += " WHERE Unites_groupe Like '%" & tb_search & "%' " &
                                "OR Unites_client LIKE '%" & tb_search & "%' " &
                                "OR Unites_famille LIKE '%" & tb_search & "%' " &
                                "OR Unites_famille_descr_FR LIKE '%" & tb_search & "%' " &
                                "OR Unites_groupe_class LIKE '%" & tb_search & "%' " &
                                "OR Unites_groupe_class_descr_FR LIKE '%" & tb_search & "%' " &
                                "OR Unites_modele LIKE '%" & tb_search & "%' " &
                                "OR Unites_serie LIKE '%" & tb_search & "%' "

            Case tb_search <> "" And WhereLine <> Nothing
                WhereLine = WhereLine.Replace(" WHERE", "")
                WhereLine = " WHERE Unites_groupe Like '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_client LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_famille LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_famille_descr_FR LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_groupe_class LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_groupe_class_descr_FR LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_modele LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Unites_serie LIKE '%" & tb_search & "%' AND" & WhereLine
        End Select

        Dim vehicules_Data As DataTable = GetData("Vehicules_Data", "vehicules", WhereLine, pageNumber, recordsPerPage)
        ViewState("dt") = vehicules_Data
        rpt_list_vehicules.DataSource = vehicules_Data
        rpt_list_vehicules.DataBind()
    End Sub

    Protected Sub foward_pnl_div_produits_ServerClick(sender As Object, e As EventArgs)
        Produits_Data("foward")
    End Sub
    Protected Sub filter_div_produits_ServerClick(sender As Object, e As EventArgs)
        Produits_Data("filter")
    End Sub
    Protected Sub back_pnl_div_produits_ServerClick(sender As Object, e As EventArgs)
        Produits_Data("back")
    End Sub
    Sub Produits_Data(scenario)

        Dim module_ As String = "Produits_Data"
        Dim pageNumber As String = produits_pageNumber.Value
        Dim recordsPerPage As String = produits_recordsPerPage.Text
        Process_Pager(pageNumber, recordsPerPage, scenario, module_)
        Update_Pagenumber(module_, pageNumber)

        Dim WhereLine As String = Nothing
        Select Case True
            Case filter_div_produits_Prod_qte.Text = "Status"
            Case filter_div_produits_Prod_qte.Text = "Tous"
            Case filter_div_produits_Prod_qte.Text = "En Inventaire"
                WhereLine = " WHERE Prod_qte > 0 "
            Case filter_div_produits_Prod_qte.Text = "En financement"
                WhereLine = " WHERE Prod_qte < 1 "
        End Select

        Dim Prod_groupe_id As String = Search_tb_Prod_groupe_id.Text
        Select Case True
            Case Prod_groupe_id = "Groupe"
            Case Prod_groupe_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_groupe_id is null "
            Case Prod_groupe_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_groupe_id is null "
        End Select

        Dim Prod_type_id As String = Search_tb_Prod_type_id.Text
        Select Case True
            Case Prod_type_id = "Type"
            Case Prod_type_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_type_id is null "
            Case Prod_type_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_type_id is null "
            Case Else
        End Select

        Dim ddpFeature_ As String = ddpFeature.Text
        Select Case True
            Case ddpFeature_ = "Status"
            Case ddpFeature_ <> "Status" And WhereLine = Nothing
                WhereLine = " WHERE Prod_photo_url1 = 'Models/images/A VENIR PCS.jpg' "
            Case ddpFeature_ <> "Status" And WhereLine <> Nothing
                WhereLine += " AND Prod_photo_url1 = 'Models/images/A VENIR PCS.jpg' "
        End Select

        Dim tb_search As String = tb_pcs.Text
        Select Case True
            Case tb_search <> "" And WhereLine = Nothing

                WhereLine += " WHERE Prod_id Like '%" & tb_search & "%' " &
                                "OR Prod_desc LIKE '%" & tb_search & "%' " &
                                "OR Prod_desca LIKE '%" & tb_search & "%' " &
                                "OR Prod_groupe_id_descr_FR LIKE '%" & tb_search & "%' " &
                                "OR Prod_groupe_id_descr_EN LIKE '%" & tb_search & "%' " &
                                "OR Prod_type_id_descr_FR LIKE '%" & tb_search & "%' " &
                                "OR Prod_type_id_descr_EN LIKE '%" & tb_search & "%' " &
                                "OR Prod_ligne LIKE '%" & tb_search & "%' "

            Case tb_search <> "" And WhereLine <> Nothing
                WhereLine = WhereLine.Replace(" WHERE", "")
                WhereLine = " WHERE Prod_id Like '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_desc LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_desca LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_groupe_id_descr_FR LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_groupe_id_descr_EN LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_type_id_descr_FR LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_type_id_descr_EN LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_ligne LIKE '%" & tb_search & "%' AND" & WhereLine
        End Select

        Select Case True
            Case ddpFeature_ = "Status"
            Case ddpFeature_ <> "Status" And WhereLine = Nothing
                WhereLine = " ORDER BY cout DESC"
            Case ddpFeature_ <> "Status" And WhereLine <> Nothing
                WhereLine = " ORDER BY cout DESC"
        End Select

        Dim produits_Data As DataTable = GetData("Produits_Data", "produits", WhereLine, pageNumber, recordsPerPage)
        ViewState("dt") = produits_Data
        rpt_list_produits.DataSource = produits_Data
        rpt_list_produits.DataBind()
    End Sub

    Protected Sub foward_pnl_div_clients_ServerClick(sender As Object, e As EventArgs)
        Clients_Data("foward")
    End Sub
    Protected Sub back_pnl_div_clients_ServerClick(sender As Object, e As EventArgs)
        Clients_Data("back")
    End Sub
    Protected Sub filter_div_clients_ServerClick(sender As Object, e As EventArgs)
        Clients_Data("filter")
    End Sub
    Sub Clients_Data(scenario)
        Dim module_ As String = "Clients_Data"
        Dim pageNumber As String = clients_pageNumber.Value
        Dim recordsPerPage As String = Clients_recordsPerPage.Text
        Process_Pager(pageNumber, recordsPerPage, scenario, module_)
        Update_Pagenumber(module_, pageNumber)
        Dim WhereLine As String = Nothing


        Dim Clients_category As String = Search_tb_Clients_category.Text
        Select Case True
            Case Clients_category = "Groupe" Or Clients_category = ""
                BindDropDownList("clients", "Search_tb_Clients_category", "Clients_category", WhereLine)

            Case Clients_category <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_category = '" & Clients_category & "' "
            Case Clients_category <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_category = '" & Clients_category & "' "
        End Select

        Dim Clients_groupe As String = Search_tb_Clients_groupe.Text
        Select Case True
            Case Clients_groupe = "Groupe" Or Clients_groupe = ""
                BindDropDownList("clients", "Search_tb_Clients_groupe", "Clients_groupe", WhereLine)

            Case Clients_groupe <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_groupe = '" & Clients_groupe & "' "
            Case Clients_groupe <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_groupe = '" & Clients_groupe & "' "
        End Select

        Dim Clients_type As String = Search_tb_clients_type_id.Text
        Select Case True
            Case Clients_type = "Type" Or Clients_type = ""
                BindDropDownList("clients", "Search_tb_Clients_type", "Clients_type", WhereLine)

            Case Clients_type <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_type = '" & Clients_type & "' "
            Case Clients_type <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_type = '" & Clients_type & "' "
            Case Else
        End Select

        Dim tb_search As String = tb_clients.Text
        Select Case True
            Case tb_search <> "" And WhereLine = Nothing

                WhereLine += " WHERE Clients_id Like '%" & tb_search & "%' " &
                                "OR Clients_comp LIKE '%" & tb_search & "%' " &
                                "OR Clients_contact LIKE '%" & tb_search & "%' " &
                                "OR Clients_groupe LIKE '%" & tb_search & "%' " &
                                "OR Clients_type LIKE '%" & tb_search & "%' " &
                                "OR Clients_category LIKE '%" & tb_search & "%' " &
                                "OR Clients_addr_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_ville_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_cp_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_prov_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_pays_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_phone_contact LIKE '%" & tb_search & "%' " &
                                "OR Clients_telcel LIKE '%" & tb_search & "%' " &
                                "OR Clients_fax LIKE '%" & tb_search & "%' " &
                                "OR Clients_fax LIKE '%" & tb_search & "%' " &
                                "OR Clients_birthday LIKE '%" & tb_search & "%' " &
                                "OR Clients_autrea LIKE '%" & tb_search & "%' " &
                                "OR Clients_territoire LIKE '%" & tb_search & "%' " &
                                "OR Clients_email LIKE '%" & tb_search & "%' " &
                                "OR Clients_email LIKE '%" & tb_search & "%' "

                BindDropDownList("clients", "Search_tb_Clients_category", "Clients_category", WhereLine)
                BindDropDownList("clients", "Search_tb_Clients_groupe", "Clients_groupe", WhereLine)
                BindDropDownList("clients", "Search_tb_Clients_type", "Clients_type", WhereLine)


            Case tb_search <> "" And WhereLine <> Nothing
                WhereLine = WhereLine.Replace(" WHERE", "")
                WhereLine = " WHERE Clients_id Like '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_comp LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_contact LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_groupe LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_type LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_category LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_addr_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_ville_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_cp_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_prov_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_pays_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_phone_contact LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_telcel LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_fax LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_fax LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_birthday LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_autrea LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_territoire LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_email LIKE '%" & tb_search & "%' AND" & WhereLine

        End Select


        Try
            Dim produits_Data As DataTable = GetData("Clients_Data", "clients", WhereLine, pageNumber, recordsPerPage)
            ViewState("dt") = produits_Data
            rpt_list_client.DataSource = produits_Data
            rpt_list_client.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub filter_div_Fournisseur_ServerClick(sender As Object, e As EventArgs)
        Fournisseur_Data("filter")
    End Sub
    Protected Sub foward_pnl_div_Fournisseur_ServerClick(sender As Object, e As EventArgs)
        Fournisseur_Data("foward")
    End Sub
    Protected Sub back_pnl_div_Fournisseur_ServerClick(sender As Object, e As EventArgs)
        Fournisseur_Data("back")
    End Sub
    Sub Fournisseur_Data(scenario)
        Dim module_ As String = "Fournisseur_Data"
        Dim pageNumber As String = clients_pageNumber.Value
        Dim recordsPerPage As String = Clients_recordsPerPage.Text
        Process_Pager(pageNumber, recordsPerPage, scenario, module_)
        Update_Pagenumber(module_, pageNumber)

        Dim WhereLine As String = Nothing


        Dim Clients_category As String = Search_tb_Fournisseur_category.Text
        Select Case True
            Case Clients_category = "Groupe" Or Clients_category = ""
                BindDropDownList("fournisseur", "Search_tb_Fournisseur_category", "Clients_category", WhereLine)

            Case Clients_category <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_category = '" & Clients_category & "' "
            Case Clients_category <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_category = '" & Clients_category & "' "
        End Select

        Dim Clients_groupe As String = Search_tb_Fournisseur_groupe.Text
        Select Case True
            Case Clients_groupe = "Groupe" Or Clients_groupe = ""
                BindDropDownList("fournisseur", "Search_tb_Fournisseur_groupe", "Clients_groupe", WhereLine)

            Case Clients_groupe <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_groupe = '" & Clients_groupe & "' "
            Case Clients_groupe <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_groupe = '" & Clients_groupe & "' "
        End Select

        Dim Clients_type As String = Search_tb_Fournisseur_type.Text
        Select Case True
            Case Clients_type = "Type" Or Clients_type = ""
                BindDropDownList("fournisseur", "Search_tb_Fournisseur_type", "Clients_type", WhereLine)

            Case Clients_type <> "" And WhereLine = Nothing
                WhereLine = " WHERE Clients_type = '" & Clients_type & "' "
            Case Clients_type <> "" And WhereLine <> Nothing
                WhereLine += " AND Clients_type = '" & Clients_type & "' "
            Case Else
        End Select

        Dim tb_search As String = tb_clients.Text
        Select Case True
            Case tb_search <> "" And WhereLine = Nothing

                WhereLine += " WHERE Clients_id Like '%" & tb_search & "%' " &
                                "OR Clients_comp LIKE '%" & tb_search & "%' " &
                                "OR Clients_contact LIKE '%" & tb_search & "%' " &
                                "OR Clients_groupe LIKE '%" & tb_search & "%' " &
                                "OR Clients_type LIKE '%" & tb_search & "%' " &
                                "OR Clients_category LIKE '%" & tb_search & "%' " &
                                "OR Clients_addr_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_ville_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_cp_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_prov_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_pays_1 LIKE '%" & tb_search & "%' " &
                                "OR Clients_phone_contact LIKE '%" & tb_search & "%' " &
                                "OR Clients_telcel LIKE '%" & tb_search & "%' " &
                                "OR Clients_fax LIKE '%" & tb_search & "%' " &
                                "OR Clients_fax LIKE '%" & tb_search & "%' " &
                                "OR Clients_birthday LIKE '%" & tb_search & "%' " &
                                "OR Clients_autrea LIKE '%" & tb_search & "%' " &
                                "OR Clients_territoire LIKE '%" & tb_search & "%' " &
                                "OR Clients_email LIKE '%" & tb_search & "%' " &
                                "OR Clients_email LIKE '%" & tb_search & "%' "

                BindDropDownList("fournisseur", "Search_tb_Fournisseur_category", "Clients_category", WhereLine)
                BindDropDownList("fournisseur", "Search_tb_Fournisseur_groupe", "Clients_groupe", WhereLine)
                BindDropDownList("fournisseur", "Search_tb_Fournisseur_type", "Clients_type", WhereLine)


            Case tb_search <> "" And WhereLine <> Nothing
                WhereLine = WhereLine.Replace(" WHERE", "")
                WhereLine = " WHERE Clients_id Like '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_comp LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_contact LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_groupe LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_type LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_category LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_addr_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_ville_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_cp_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_prov_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_pays_1 LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_phone_contact LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_telcel LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_fax LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_fax LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_birthday LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_autrea LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_territoire LIKE '%" & tb_search & "%' AND" & WhereLine &
                                "OR Clients_email LIKE '%" & tb_search & "%' AND" & WhereLine

        End Select


        Try
            Dim produits_Data As DataTable = GetData("Fournisseur_Data", "fournisseur", WhereLine, pageNumber, recordsPerPage)
            ViewState("dt") = produits_Data
            rpt_list_fournisseur.DataSource = produits_Data
            rpt_list_fournisseur.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub filter_div_montage_ServerClick(sender As Object, e As EventArgs)
        Montage_Data("filter")
    End Sub
    Protected Sub back_pnl_div_montage_ServerClick(sender As Object, e As EventArgs)
        Montage_Data("back")
    End Sub
    Protected Sub foward_pnl_div_montage_ServerClick(sender As Object, e As EventArgs)
        Montage_Data("foward")
    End Sub
    Sub Montage_Data(scenario)

        Dim module_ As String = "Montage_Data"
        Dim pageNumber As String = montage_pageNumber.Value
        Dim recordsPerPage As String = montage_recordsPerPage.Text
        Process_Pager(pageNumber, recordsPerPage, scenario, module_)
        Update_Pagenumber(module_, pageNumber)

        Dim WhereLine As String = Nothing

        Dim Prod_famille_id As String = tb_montage_Prod_famille_id.Text
        Try
            Dim parts As String() = Prod_famille_id.Split(New String() {" : "}, StringSplitOptions.None)
            Prod_famille_id = parts(0)
        Catch ex As Exception

        End Try

        Select Case True
            Case Prod_famille_id = "Famille"
            Case Prod_famille_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_famille_id = '" & Prod_famille_id & "' "
            Case Prod_famille_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_famille_id = '" & Prod_famille_id & "' "
        End Select


        Dim Prod_groupe_id As String = tb_montage_Prod_groupe_id.Text
        Try
            Dim parts As String() = Prod_groupe_id.Split(New String() {" : "}, StringSplitOptions.None)
            Prod_groupe_id = parts(0)
        Catch ex As Exception

        End Try
        Select Case True
            Case Prod_groupe_id = "" Or Prod_groupe_id = "Groupe"
                BindDropDownList("style_catalogue", "tb_montage_Prod_groupe_id", "Prod_groupe_id", WhereLine)

            Case Prod_groupe_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_groupe_id = '" & Prod_groupe_id & "' "

            Case Prod_groupe_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_groupe_id = '" & Prod_groupe_id & "' "
        End Select



        Dim Prod_type_id As String = tb_montage_Prod_type_id.Text
        Try
            Dim parts As String() = Prod_type_id.Split(New String() {" : "}, StringSplitOptions.None)
            Prod_type_id = parts(0)
        Catch ex As Exception

        End Try
        Select Case True
            Case Prod_type_id = "" Or Prod_type_id = "Type"
                BindDropDownList("style_catalogue", "tb_montage_Prod_type_id", "Prod_type_id", WhereLine)

            Case Prod_type_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_type_id = '" & Prod_type_id & "' "

            Case Prod_type_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_type_id = '" & Prod_type_id & "' "

            Case Else

        End Select

        Dim Prod_categorie_id As String = tb_montage_Prod_categorie_id.Text
        Try
            Dim parts As String() = Prod_categorie_id.Split(New String() {" : "}, StringSplitOptions.None)
            Prod_categorie_id = parts(0)
        Catch ex As Exception

        End Try
        Select Case True
            Case Prod_categorie_id = "" Or Prod_categorie_id = "Categorie"
                BindDropDownList("style_catalogue", "tb_montage_Prod_categorie_id", "Prod_categorie_id", WhereLine)

            Case Prod_categorie_id <> "" And WhereLine = Nothing
                WhereLine = " WHERE Prod_categorie_id = '" & Prod_categorie_id & "' "

            Case Prod_categorie_id <> "" And WhereLine <> Nothing
                WhereLine += " AND Prod_categorie_id = '" & Prod_categorie_id & "' "

            Case Else

        End Select

        Dim tb_search As String = tb_montage.Value
        Select Case True
            Case tb_search <> "" And WhereLine = Nothing

                WhereLine += " WHERE Prod_style_id Like '%" & tb_search & "%' " &
                                "OR Prod_Description_fr LIKE '%" & tb_search & "%' " &
                                "OR Prod_Description_en LIKE '%" & tb_search & "%' " &
                                "OR Prod_fam_style_fr LIKE '%" & tb_search & "%' " &
                                "OR Prod_fam_style_en LIKE '%" & tb_search & "%' " &
                                "OR Prod_cat_style_fr LIKE '%" & tb_search & "%' " &
                                "OR Prod_cat_style_en LIKE '%" & tb_search & "%' "

                BindDropDownList("style_catalogue", "tb_montage_Prod_type_id", "Prod_type_id", WhereLine)
                BindDropDownList("style_catalogue", "tb_montage_Prod_groupe_id", "Prod_groupe_id", WhereLine)
                BindDropDownList("style_catalogue", "tb_montage_Prod_categorie_id", "Prod_categorie_id", WhereLine)


            Case tb_search <> "" And WhereLine <> Nothing
                WhereLine = WhereLine.Replace(" WHERE", "")
                WhereLine = " WHERE Prod_style_id Like '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_Description_fr LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_Description_en LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_fam_style_fr LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_fam_style_en LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_cat_style_fr LIKE '%" & tb_search & "%' AND" & WhereLine &
                                 "OR Prod_cat_style_en LIKE '%" & tb_search & "%' AND" & WhereLine

                BindDropDownList("style_catalogue", "tb_montage_Prod_type_id", "Prod_type_id", WhereLine)
                BindDropDownList("style_catalogue", "tb_montage_Prod_groupe_id", "Prod_groupe_id", WhereLine)
                BindDropDownList("style_catalogue", "tb_montage_Prod_categorie_id", "Prod_categorie_id", WhereLine)

        End Select

        Dim montage_Data As DataTable = GetData("Montage_Data", "style_catalogue", WhereLine, pageNumber, recordsPerPage)
        ViewState("dt") = montage_Data
        rpt_list_montage.DataSource = montage_Data
        rpt_list_montage.DataBind()


    End Sub

    Private Sub BindDropDownList(dbBase As String, dropDownListId As String, columnName As String, WhereLine As String)
        Try
            Dim tableName As String = dbBase ' Remplacez par le nom réel de votre table
            Dim data As DataTable = GetDistinctColumnData(tableName, columnName, WhereLine)

            Dim ddl As DropDownList

            ' Sélection du DropDownList approprié
            Select Case True
                Case dropDownListId = "Search_tb_Fournisseur_category"
                    ddl = Search_tb_Fournisseur_category
                Case dropDownListId = "Search_tb_Fournisseur_groupe"
                    ddl = Search_tb_Fournisseur_groupe
                Case dropDownListId = "Search_tb_Fournisseur_type"
                    ddl = Search_tb_Fournisseur_type

                Case dropDownListId = "Search_tb_Clients_category"
                    ddl = Search_tb_Clients_category
                Case dropDownListId = "Search_tb_Clients_groupe"
                    ddl = Search_tb_Clients_groupe
                Case dropDownListId = "Search_tb_Clients_type"
                    ddl = Search_tb_clients_type_id

                Case dropDownListId = "tb_montage_Prod_groupe_id"
                    ddl = tb_montage_Prod_groupe_id
                Case dropDownListId = "tb_montage_Prod_type_id"
                    ddl = tb_montage_Prod_type_id
                Case dropDownListId = "tb_montage_Prod_categorie_id"
                    ddl = tb_montage_Prod_categorie_id
                Case Else
                    dropDownListId = dropDownListId
                    ' Gérer le cas où l'ID fourni ne correspond à aucun DropDownList connu
                    Return
            End Select

            ' Ajoutez une option par défaut si nécessaire
            ddl.Items.Clear()
            ddl.Items.Add(New ListItem("Sélectionnez", ""))

            For Each row As DataRow In data.Rows
                Dim value As String = row(columnName).ToString()
                Dim listItem As New ListItem(value, value)
                ddl.Items.Add(listItem)
            Next

            Select Case True
                Case dropDownListId = "Search_tb_Clients_category"
                    ddl.Items.Add(New ListItem("Vide", ""))

                Case dropDownListId = "Search_tb_Clients_groupe"
                    ddl.Items.Add(New ListItem("Vide", ""))

                Case dropDownListId = "Search_tb_Clients_type"
                    ddl.Items.Add(New ListItem("Vide", ""))

                Case dropDownListId = "tb_montage_Prod_groupe_id"

                Case dropDownListId = "tb_montage_Prod_type_id"

                Case dropDownListId = "tb_montage_Prod_categorie_id"

                Case Else
                    dropDownListId = dropDownListId
                    ' Gérer le cas où l'ID fourni ne correspond à aucun DropDownList connu
                    Return
            End Select

        Catch ex As Exception
            ' Idéalement, loguez l'exception ou affichez un message d'erreur
        End Try
    End Sub
    Public Function GetDistinctColumnData(tableName As String, columnName As String, Optional whereClause As String = "") As DataTable
        Dim query As String
        If String.IsNullOrEmpty(whereClause) Then
            query = String.Format("SELECT DISTINCT {0} FROM {1} ORDER BY {0} ASC", columnName, tableName)
        Else
            ' Assurez-vous que whereClause commence par "WHERE"
            query = String.Format("SELECT DISTINCT {0} FROM {1} {2} ORDER BY {0} ASC", columnName, tableName, whereClause)
        End If

        ' Utilisez votre méthode existante pour obtenir les données
        ' Cette méthode doit exécuter la requête SQL et retourner un DataTable avec les résultats
        Dim dataTable As DataTable = GetDataQuery(tableName, query)
        Return dataTable
    End Function


#End Region
#Region "FORM MANAGER NAVIGATION"
    Sub reset_pnl_clients_()
        pnl_clients_montage.Visible = False
        pnl_clients_coordonne.Visible = False
        pnl_clients_supplementaire.Visible = False
        pnl_clients_recherche.Visible = False
    End Sub
    Protected Sub btn_pnl_clients_montage_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_clients_()
        pnl_clients_montage.Visible = True
    End Sub
    Protected Sub btn_pnl_clients_coordonne_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_clients_()
        pnl_clients_coordonne.Visible = True
    End Sub
    Protected Sub btn_pnl_clients_supplementaire_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_clients_()
        pnl_clients_supplementaire.Visible = True
    End Sub
    Protected Sub btn_pnl_clients_recherche_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_clients_()
        pnl_clients_recherche.Visible = True

    End Sub
    Sub reset_pnl_fournisseur_()
        pnl_kt_tab_pane_fournisseur_montage.Visible = False
        pnl_kt_tab_pane_fournisseur_recherche.Visible = False
        pnl_kt_tab_pane_fournisseur_Coordonne.Visible = False
        pnl_kt_tab_pane_fournisseur_Autres.Visible = False
    End Sub
    Protected Sub btn_pnl_fournisseur_recherche_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_fournisseur_()
        pnl_kt_tab_pane_fournisseur_recherche.Visible = True
    End Sub
    Protected Sub btn_pnl_fournisseur_information_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_fournisseur_()
        pnl_kt_tab_pane_fournisseur_montage.Visible = True
    End Sub
    Protected Sub btn_pnl_fournisseur_Coordonne_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_fournisseur_()
        pnl_kt_tab_pane_fournisseur_Coordonne.Visible = True
    End Sub
    Protected Sub btn_pnl_fournisseur_Autres_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_fournisseur_()
        pnl_kt_tab_pane_fournisseur_Autres.Visible = True
    End Sub
    Sub reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_1.Visible = False
        pnl_kt_tab_pane_vehicules_montage_2.Visible = False
        pnl_kt_tab_pane_vehicules_montage_3.Visible = False
        pnl_kt_tab_pane_vehicules_montage_4.Visible = False
        pnl_kt_tab_pane_vehicules_montage_5.Visible = False
        pnl_kt_tab_pane_vehicules_montage_6.Visible = False
        pnl_kt_tab_pane_vehicules_montage_7.Visible = False
        pnl_kt_tab_pane_vehicules_recherche.Visible = False
    End Sub
    Protected Sub btn_pnl_vehicules_recherche_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_recherche.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_1_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_1.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_2_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_2.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_3_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_3.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_4_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_4.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_5_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_5.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_6_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_6.Visible = True
    End Sub
    Protected Sub btn_pnl_kt_tab_pane_vehicules_montage_7_ServerClick(sender As Object, e As EventArgs)
        reset_pnl_vehicules_()
        pnl_kt_tab_pane_vehicules_montage_7.Visible = True
    End Sub


    Sub rpt_load_event_GRO(CommandName_, Variable_)
        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_id"

        Dim Value_ As String = GetColumnValueFromDB("style_catalogue", "Prod_groupe_id", Variable_, "Prod_Description_fr", "GMBGMB")
        Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")
    End Sub
    Sub rpt_load_event_SEX(CommandName_, Variable_)
        Dim Value_ As String = "Y"
        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_sexe"

        Select Case True
            Case CommandName_ = "NO_SEX"
                Value_ = "NO"
            Case Else
                Value_ = CommandName_
        End Select
        Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")
    End Sub
    Sub rpt_load_event_VET(CommandName_, Variable_)
        Dim Value_ As String = "Y"
        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_style"
        Value_ = CommandName_

        Dim Prod_style As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_style", "GMBGMB")
        Dim Prod_Style_slingshot As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_slingshot", "GMBGMB")
        Dim Prod_Style_scooter As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_scooter", "GMBGMB")
        Dim Prod_Style_atv As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_atv", "GMBGMB")
        Dim Prod_Style_snow As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_snow", "GMBGMB")
        Dim Prod_Style_motorcycle As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_motorcycle", "GMBGMB")
        Dim Prod_Style_motocross As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_motocross", "GMBGMB")
        Dim Prod_Style_marine As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_marine", "GMBGMB")
        Dim Prod_Style_SCIE100 As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_SCIE100", "GMBGMB")

        If Prod_style <> CommandName_ Then
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")

            Prod_Style_slingshot = Prod_Style_slingshot.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_slingshot, "Prod_Style_slingshot", "gmb")

            Prod_Style_scooter = Prod_Style_scooter.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_scooter, "Prod_Style_scooter", "gmb")

            Prod_Style_atv = Prod_Style_atv.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_atv, "Prod_Style_atv", "gmb")

            Prod_Style_snow = Prod_Style_snow.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_snow, "Prod_Style_snow", "gmb")

            Prod_Style_motorcycle = Prod_Style_motorcycle.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_motorcycle, "Prod_Style_motorcycle", "gmb")

            Prod_Style_motocross = Prod_Style_motocross.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_motocross, "Prod_Style_motocross", "gmb")

            Prod_Style_marine = Prod_Style_marine.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_marine, "Prod_Style_marine", "gmb")

            Prod_Style_SCIE100 = Prod_Style_SCIE100.Replace(Prod_style, CommandName_)
            Where_To_Update(DBSelection, FieldVariable_, Variable_, Prod_Style_SCIE100, "Prod_Style_SCIE100", "gmb")
        End If


    End Sub
    Sub rpt_load_event_HUM(CommandName_, Variable_)
        Dim Value_ As String = "Y"
        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_genre"
        Select Case True
            Case CommandName_ = "NO_HUM"
                Value_ = "HUM005"
            Case Else
                Value_ = CommandName_
        End Select
        Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")
    End Sub

    Sub rpt_load_event_SAI(CommandName_, Variable_)
        Dim Value_ As String = "Y"
        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_id"
        Dim SAI1 As String = "Prod_saison_hiver"
        Dim SAI2 As String = "Prod_saison_printemps"
        Dim SAI3 As String = "Prod_saison_ete"
        Dim SAI4 As String = "Prod_saison_automne"
        Select Case True
            Case CommandName_ = "NO_SEASON"
                Value_ = "NO"
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, SAI1, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, SAI2, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, SAI3, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, SAI4, "gmb")
                Exit Sub
            Case CommandName_ = "SAI1"
                FieldName_ = SAI1

            Case CommandName_ = "SAI2"
                FieldName_ = SAI2

            Case CommandName_ = "SAI3"
                FieldName_ = SAI3

            Case CommandName_ = "SAI4"
                FieldName_ = SAI4
        End Select
        Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")
    End Sub
    Sub rpt_load_event_CAT(CommandName_, Variable_)
        Dim Value_ As String = "YES"

        Dim Prod_style As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_style", "GMBGMB")

        Dim Prod_Style_slingshot As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_slingshot", "GMBGMB")
        Dim Prod_Style_scooter As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_scooter", "GMBGMB")
        Dim Prod_Style_atv As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_atv", "GMBGMB")
        Dim Prod_Style_snow As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_snow", "GMBGMB")
        Dim Prod_Style_motorcycle As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_motorcycle", "GMBGMB")
        Dim Prod_Style_motocross As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_motocross", "GMBGMB")
        Dim Prod_Style_marine As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_marine", "GMBGMB")
        Dim Prod_Style_SCIE100 As String = GetColumnValueFromDB("produits", "Prod_id", Variable_, "Prod_Style_SCIE100", "GMBGMB")

        Dim DBSelection As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Dim FieldName_ As String = "Prod_id"
        Dim CAT1 As String = "Prod_Style_motocross"
        Dim CAT2 As String = "Prod_Style_motorcycle"
        Dim CAT3 As String = "Prod_Style_atv"
        Dim CAT4 As String = "Prod_Style_snow"
        Dim CAT5 As String = "Prod_Style_SCIE100"

        If Prod_Style_motocross.Contains("Y") And Prod_Style_motorcycle.Contains("Y") And Prod_Style_atv.Contains("Y") And Prod_Style_snow.Contains("Y") And Prod_Style_motocross.Contains("Y") And Prod_Style_SCIE100.Contains("Y") Then
            Where_To_Update(DBSelection, FieldVariable_, Variable_, "NO" & Prod_style, CAT1, "gmb")
            Where_To_Update(DBSelection, FieldVariable_, Variable_, "NO" & Prod_style, CAT2, "gmb")
            Where_To_Update(DBSelection, FieldVariable_, Variable_, "NO" & Prod_style, CAT3, "gmb")
            Where_To_Update(DBSelection, FieldVariable_, Variable_, "NO" & Prod_style, CAT4, "gmb")
            Where_To_Update(DBSelection, FieldVariable_, Variable_, "YES" & Prod_style, CAT5, "gmb")
        Else
            Select Case True
                Case CommandName_ = "CAT1" And Prod_Style_motocross.Contains("Y")
                    Value_ = "NO"
                Case CommandName_ = "CAT2" And Prod_Style_motorcycle.Contains("Y")
                    Value_ = "NO"
                Case CommandName_ = "CAT3" And Prod_Style_atv.Contains("Y")
                    Value_ = "NO"
                Case CommandName_ = "CAT4" And Prod_Style_snow.Contains("Y")
                    Value_ = "NO"
                Case CommandName_ = "CAT5" And Prod_Style_SCIE100.Contains("Y")
                    Value_ = "NO"
            End Select
        End If



        Value_ = Value_ & Prod_style

        Select Case True
            Case CommandName_ = "NO_CAT"
                Value_ = "YES" & Prod_style
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, CAT1, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, CAT2, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, CAT3, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, CAT4, "gmb")
                Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, CAT5, "gmb")
                Exit Sub
            Case CommandName_ = "CAT1"
                FieldName_ = CAT1
            Case CommandName_ = "CAT2"
                FieldName_ = CAT2
            Case CommandName_ = "CAT3"
                FieldName_ = CAT3
            Case CommandName_ = "CAT4"
                FieldName_ = CAT4
            Case CommandName_ = "CAT5"
                FieldName_ = CAT5
        End Select
        Where_To_Update(DBSelection, FieldVariable_, Variable_, Value_, FieldName_, "gmb")

    End Sub

    Protected Sub btn_Panel_MontageWeb_Fiche_back_ServerClick(sender As Object, e As EventArgs)
        Panel_MontageWeb.Visible = True
        Panel_Fiche_Produit.Visible = False
    End Sub
    Protected Sub btn_Panel_Fiche_Produit_Trouver_Click(sender As Object, e As EventArgs)
        Load_Fiche_Produits(Panel_Fiche_Produit_id_chercher.Value)
    End Sub
    Sub Load_Fiche_Produits(Prod_id)
        Produit_Compatible_1.Visible = False
        Produit_Compatible_2.Visible = False
        Produit_Compatible_3.Visible = False
        Produit_Compatible_4.Visible = False
        Produit_Compatible_5.Visible = False
        section_groupe.Visible = False
        section_type.Visible = False
        section_categorie.Visible = False
        section_Saison.Visible = False
        section_Format.Visible = False
        section_Sexe.Visible = False
        section_prod_genre.Visible = False
        Try

            Load_Panel_Fiche_Produit_ddl()

            Dim Database As String = "produits"
            Dim WhereLine As String = " WHERE Prod_id = '" & Prod_id & "'"
            Dim Get_Data_2 As DataTable = GetData("rpt_load_event_ItemCommand", Database, WhereLine, 1, 10000)
            For Each row As DataRow In Get_Data_2.Rows
                Dim Prod_desc As String = row("Prod_desc").ToString()
                Panel_Fiche_Produit_Description.Value = Prod_desc
                Panel_Fiche_Produit_id.InnerText = Prod_id
                Panel_Fiche_Produit_id_chercher.Value = Prod_id

                Dim Prod_prix_1 As String = row("Prod_prix_1").ToString()
                Panel_Fiche_Produit_Prix.Value = Prod_prix_1
                Dim Prod_qte As String = row("Prod_qte").ToString()
                Panel_Fiche_Produit_Quantite.Value = Prod_qte

                Dim Prod_famille_id As String = row("Prod_famille_id").ToString().Trim()
                Dim Prod_style As String
                Dim Prod_Description_fr As String
                Select Case Prod_famille_id
                    Case "PNE", "TYP", "VET"
                        section_groupe.Visible = True
                        Select Case True
                            Case Prod_famille_id = "PNE"
                                ddl_Panel_Fiche_Produit_Famille.Text = "Pneus"
                            Case Prod_famille_id = "VET"
                                ddl_Panel_Fiche_Produit_Famille.Text = "Vetements"
                            Case Prod_famille_id = "TYP"
                                ddl_Panel_Fiche_Produit_Famille.Text = "Pieces et Accessoires"
                        End Select
                        Prod_style = row("Prod_style").ToString().Trim()
                        Load_Groupe("Prod_groupe_id", Prod_famille_id)
                        Produit_Compatible_1.Visible = True

                        Dim Prod_couleur As String = row("Prod_couleur").ToString()
                        Panel_Fiche_Produit_Prod_couleur.Value = Prod_couleur.ToUpper

                        If Prod_style <> Nothing Then
                            section_type.Visible = True
                            Prod_Description_fr = GetColumnValueFromDB("style_catalogue", "Prod_groupe_id", Prod_style, "Prod_Description_fr", "GSIGSI")
                            ddl_Panel_Fiche_Produit_Famille.Text = Prod_Description_fr
                            ddl_Panel_Fiche_Produit_Groupe.Text = Prod_Description_fr & " " & Prod_style


                            Load_Groupe("Prod_type_id", Prod_style)
                            Dim Prod_type_id As String = row("Prod_type_id").ToString().Trim()
                            If Prod_type_id <> Nothing Then
                                Prod_Description_fr = GetColumnValueFromDB("style_catalogue", "Prod_type_id", Prod_type_id, "Prod_fam_style_fr", "GSIGSI")
                                If Prod_Description_fr <> Nothing Then
                                    ddl_Panel_Fiche_Produit_Type.Text = Prod_Description_fr & " " & Prod_type_id
                                End If

                                section_categorie.Visible = True
                                Load_Groupe("Prod_categorie_id", Prod_type_id)
                                Dim Prod_categorie_id As String = row("Prod_categorie_id").ToString().Trim()
                                If Prod_categorie_id <> Nothing And Prod_categorie_id <> "Nothing" Then
                                    Prod_Description_fr = GetColumnValueFromDB("style_catalogue", "Prod_categorie_id", Prod_categorie_id, "Prod_fam_style_fr", "GSIGSI")
                                    If Prod_Description_fr <> Nothing And Prod_Description_fr <> "Nothing" Then
                                        ddl_Panel_Fiche_Produit_Categorie.Text = Prod_Description_fr & " " & Prod_type_id
                                    End If
                                End If

                            End If



                        End If

                End Select

                Select Case True
                    Case Prod_famille_id = "TYP"
                        ChargerDonneesEtTrier()
                        Load_Compatibilite_Produits(row("Prod_Style_slingshot").ToString(), row("Prod_Style_scooter").ToString(), row("Prod_Style_atv").ToString(),
                                row("Prod_Style_snow").ToString(), row("Prod_Style_motorcycle").ToString(), row("Prod_Style_motocross").ToString(),
                                row("Prod_Style_marine").ToString(), row("Prod_Style_SCIE100").ToString())

                    Case Prod_famille_id = "VET"
#Region "VET"
                        Dim Prod_genre As String = row("Prod_genre").ToString()
                        If Prod_genre <> Nothing Then
                            ddl_Panel_Fiche_Produit_Prod_genre.Text = Prod_genre
                        End If
                        section_prod_genre.Visible = True
                        section_Saison.Visible = True
                        section_Format.Visible = True
                        section_Sexe.Visible = True

                        Dim Prod_sexe As String = row("Prod_sexe").ToString()

                        Select Case True
                            Case Prod_sexe = "SEX001"
                                Prod_sexe = "HOMMES " & Prod_sexe

                            Case Prod_sexe = "SEX002"
                                Prod_sexe = "FEMMES " & Prod_sexe

                            Case Prod_sexe = "SEX003"
                                Prod_sexe = "ENFANTS " & Prod_sexe

                            Case Prod_sexe = "SEX004"
                                Prod_sexe = "JEUNES " & Prod_sexe

                            Case Prod_sexe = "SEX005"
                                Prod_sexe = "UNISEXE " & Prod_sexe

                        End Select
                        Try
                            ddl_Panel_Fiche_Produit_Prod_sexe.Text = Prod_sexe
                        Catch ex As Exception

                        End Try

                        Dim Prod_saison As String
                        Dim Prod_saison_hiver As String = row("Prod_saison_hiver").ToString()
                        Dim Prod_saison_printemps As String = row("Prod_saison_printemps").ToString()
                        Dim Prod_saison_ete As String = row("Prod_saison_ete").ToString()
                        Dim Prod_saison_automne As String = row("Prod_saison_automne").ToString()

                        Select Case True
                            Case Prod_saison_hiver = "YES" And Prod_saison_printemps = "YES" And Prod_saison_ete = "YES" And Prod_saison_automne = "YES"
                                Prod_saison = "TOUS LES SAISONS SAI004"
                            Case Prod_saison_printemps = "YES" And Prod_saison_automne = "YES"
                                Prod_saison = "PRINTEMPS/AUTOMNE SAI003"
                            Case Prod_saison_hiver = "YES"
                                Prod_saison = "HIVER SAI002"
                            Case Prod_saison_ete = "YES"
                                Prod_saison = "ETE SAI001"
                        End Select

                        Try
                            ddl_Panel_Fiche_Produit_Prod_saison.Text = Prod_saison
                        Catch ex As Exception

                        End Try

                        Dim Prod_grandeur As String = row("Prod_grandeur").ToString()
                        Dim Prod_grandeur_fr As String = GetColumnValueFromDB("style_catalogue", "Prod_groupe_id", Prod_grandeur, "Prod_Description_fr", "GSIGSI")
                        Select Case True
                            Case Prod_grandeur <> Nothing
                                Prod_grandeur_fr = ""

                        End Select
                        Try
                            If Prod_grandeur_fr <> Nothing Then
                                ddl_Panel_Fiche_Produit_Prod_grandeur.Text = Prod_grandeur_fr & " " & Prod_grandeur
                            End If
                        Catch ex As Exception

                        End Try




#End Region

                End Select

                Dim Prod_longueur As String = row("Prod_longueur").ToString()
                Panel_Fiche_Produit_Prod_longueur.Value = Prod_longueur
                Dim Prod_largeur As String = row("Prod_largeur").ToString()
                Panel_Fiche_Produit_Prod_largeur.Value = Prod_largeur
                Dim Prod_hauteur As String = row("Prod_hauteur").ToString()
                Panel_Fiche_Produit_Prod_hauteur.Value = Prod_hauteur
                Dim Prod_poids As String = row("Prod_poids").ToString()
                Panel_Fiche_Produit_Prod_poids.Value = Prod_poids
                Dim Prod_Actif As String = row("Prod_Actif").ToString()
                Select Case True
                    Case Prod_Actif = "true"
                        Prod_Actif = "En ligne"
                    Case Else
                        Prod_Actif = "Inactif"
                End Select
                Panel_Fiche_Produit_Statut.Text = Prod_Actif

            Next
        Catch ex As Exception

        End Try
    End Sub

    Sub Load_Groupe(scenario_, Prod_famille_id)
        section_groupe.Visible = True
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)

            Dim stg_ As String

            Select Case scenario_
                Case "Prod_groupe_id"
                    stg_ = "Choisir Groupe"
                    ddl_Panel_Fiche_Produit_Groupe.Items.Clear()
                Case "Prod_type_id"
                    stg_ = "Choisir Type"
                    ddl_Panel_Fiche_Produit_Type.Items.Clear()
                Case "Prod_categorie_id"
                    stg_ = "Choisir Categorie"
                    ddl_Panel_Fiche_Produit_Categorie.Items.Clear()
            End Select
            list_.Add(stg_)
            Dim Get_Data_ As New DataTable

            Select Case scenario_
                Case "Prod_groupe_id"
                    Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_groupe_id LIKE '%" & Prod_famille_id & "%' ORDER BY Prod_groupe_id ASC", 1, 10000)
                Case "Prod_type_id"
                    Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_type_id LIKE '%" & Prod_famille_id & "%' ORDER BY Prod_type_id ASC", 1, 10000)
                Case "Prod_categorie_id"
                    Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_categorie_id LIKE '%" & Prod_famille_id & "%' ORDER BY Prod_categorie_id ASC", 1, 10000)
            End Select

            For Each row As DataRow In Get_Data_.Rows
                Dim nom As String
                Dim employe As String
                Select Case scenario_
                    Case "Prod_groupe_id"
                        employe = row("Prod_groupe_id").ToString.Trim
                        nom = row("Prod_Description_fr").ToString.Trim
                    Case "Prod_type_id"
                        employe = row("Prod_type_id").ToString.Trim
                        nom = row("Prod_fam_style_fr").ToString.Trim
                    Case "Prod_categorie_id"
                        employe = row("Prod_categorie_id").ToString.Trim
                        nom = row("Prod_cat_style_fr").ToString.Trim
                End Select

                If nom <> Nothing Then
                    Dim item As String = nom & " " & employe

                    If nom <> Nothing And nom <> "Nothing" Then
                        If Not list_2.Contains(nom) Then
                            list_2.Add(nom)
                            list_.Add(item)
                        End If
                    End If

                End If


            Next


            ' Ajouter les éléments triés à ddl_Panel_Fiche_Produit_Prod_style
            For Each item As String In list_

                Select Case scenario_
                    Case "Prod_groupe_id"
                        ddl_Panel_Fiche_Produit_Groupe.Items.Add(item)
                    Case "Prod_type_id"
                        ddl_Panel_Fiche_Produit_Type.Items.Add(item)
                    Case "Prod_categorie_id"
                        ddl_Panel_Fiche_Produit_Categorie.Items.Add(item)
                End Select

            Next

            Select Case scenario_
                Case "Prod_groupe_id"
                    ddl_Panel_Fiche_Produit_Groupe.Text = stg_
                Case "Prod_type_id"
                    ddl_Panel_Fiche_Produit_Type.Text = stg_
                Case "Prod_categorie_id"
                    ddl_Panel_Fiche_Produit_Categorie.Text = stg_
            End Select



        Catch ex As Exception

        End Try
    End Sub
    Sub Load_Compatibilite_Produits(Prod_Style_slingshot, Prod_Style_scooter, Prod_Style_atv,
                                    Prod_Style_snow, Prod_Style_motorcycle, Prod_Style_motocross,
                                    Prod_Style_marine, Prod_Style_SCIE100)

        If Prod_Style_slingshot <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_slingshot & ", "
        End If

        If Prod_Style_scooter <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_scooter & ", "
        End If

        If Prod_Style_atv <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_atv & ", "
        End If
        If Prod_Style_snow <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_snow & ", "
        End If
        If Prod_Style_motorcycle <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_motorcycle & ", "
        End If
        If Prod_Style_motocross <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_motocross & ", "
        End If
        If Prod_Style_marine <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_marine & ", "
        End If
        If Prod_Style_slingshot <> Nothing Then
            'List_Compatible.InnerText += Prod_Style_SCIE100 & ", "
        End If
    End Sub
    Protected Sub btn_save_all_ServerClick(sender As Object, e As EventArgs)
        Dim DBBase As String = "nor"
        Dim stg_ As String
        Dim Variable_ As String = Panel_Fiche_Produit_id.InnerText
        Dim DBSelection_ As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"





        Dim Prod_famille_id As String = ddl_Panel_Fiche_Produit_Famille.Text
        Dim Prod_famille_id_descr_FR As String = ddl_Panel_Fiche_Produit_Famille.Text
        Dim Prod_famille_id_descr_EN As String = ddl_Panel_Fiche_Produit_Famille.Text
        Select Case Prod_famille_id
            Case "Vetements"
                Prod_famille_id = "VET"
                Prod_famille_id_descr_FR = "VET"
                Prod_famille_id_descr_EN = "VET"
            Case "Pneus"
                Prod_famille_id = "PNE"
                Prod_famille_id_descr_FR = "PNE"
                Prod_famille_id_descr_EN = "PNE"
            Case "Pieces et Accessoires"
                Prod_famille_id = "TYP"
                Prod_famille_id_descr_FR = "Pieces et Accessoires"
                Prod_famille_id_descr_EN = "Parts and Accessories"
        End Select
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_famille_id, "Prod_famille_id", DBBase)
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_famille_id_descr_FR, "Prod_famille_id_descr_FR", DBBase)
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_famille_id_descr_EN, "Prod_famille_id_descr_EN", DBBase)

        Dim Prod_style As String
        stg_ = ddl_Panel_Fiche_Produit_Groupe.Text
        If stg_ <> "" And stg_ <> "Choisir Groupe" Then
            Dim Prod_groupe_id_descr_FR As String = stg_.Substring(0, stg_.Length - 7)
            Prod_style = stg_.Substring(stg_.Length - 6, 6)
            Dim Prod_groupe_id_descr_EN As String = GetColumnValueFromDB("style_catalogue", "Prod_groupe_id", Prod_style, "Prod_style_id", "GSIGSI")
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_groupe_id_descr_FR, "Prod_groupe_id_descr_FR", DBBase)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_groupe_id_descr_EN, "Prod_groupe_id_descr_EN", DBBase)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_style, "Prod_groupe_id", DBBase)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_style, "Prod_style", DBBase)
        End If

        stg_ = ddl_Panel_Fiche_Produit_Type.Text
        If stg_ <> "" Then
            Dim Prod_type_id_descr_FR As String = stg_.Substring(0, stg_.Length - 10)
            Prod_style = stg_.Substring(stg_.Length - 9, 9)
            Dim Prod_type_id_descr_EN As String = GetColumnValueFromDB("style_catalogue", "Prod_type_id", Prod_style, "Prod_style_id", "GSIGSI")
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_type_id_descr_FR, "Prod_type_id_descr_FR", DBBase)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_type_id_descr_EN, "Prod_type_id_descr_EN", DBBase)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_style, "Prod_type_id", DBBase)
        End If

        stg_ = ddl_Panel_Fiche_Produit_Categorie.Text
        If stg_ <> "" And stg_ <> "Choisir Categorie" Then
            Dim Prod_categorie_id_descr_FR As String = stg_.Substring(0, stg_.Length - 13)
            Prod_style = stg_.Substring(stg_.Length - 12, 12)
            If Prod_categorie_id_descr_FR <> "" And Prod_categorie_id_descr_FR <> "Nothing" Then
                Dim Prod_categorie_id_descr_EN As String = GetColumnValueFromDB("style_catalogue", "Prod_categorie_id", Prod_style, "Prod_style_id", "GSIGSI")
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_categorie_id_descr_FR, "Prod_categorie_id_descr_FR", DBBase)
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_categorie_id_descr_EN, "Prod_categorie_id_descr_EN", DBBase)
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_style, "Prod_categorie_id", DBBase)
            End If

        End If

        Create_Save_Produits_compatible(DBBase, produits_compatible_id_1.InnerText, ddl_Produit_Compatible_Type_1.Text,
                                        Panel_Fiche_Produit_Compatible_Annee_1.Text, Panel_Fiche_Produit_Compatible_Marque_1.Text, Panel_Fiche_Produit_Compatible_Modele_1.Text)

        Create_Save_Produits_compatible(DBBase, produits_compatible_id_2.InnerText, ddl_Produit_Compatible_Type_2.Text,
                                        Panel_Fiche_Produit_Compatible_Annee_2.Text, Panel_Fiche_Produit_Compatible_Marque_2.Text, Panel_Fiche_Produit_Compatible_Modele_2.Text)

        Create_Save_Produits_compatible(DBBase, produits_compatible_id_3.InnerText, ddl_Produit_Compatible_Type_3.Text,
                                        Panel_Fiche_Produit_Compatible_Annee_3.Text, Panel_Fiche_Produit_Compatible_Marque_3.Text, Panel_Fiche_Produit_Compatible_Modele_3.Text)

        Create_Save_Produits_compatible(DBBase, produits_compatible_id_4.InnerText, ddl_Produit_Compatible_Type_4.Text,
                                        Panel_Fiche_Produit_Compatible_Annee_4.Text, Panel_Fiche_Produit_Compatible_Marque_4.Text, Panel_Fiche_Produit_Compatible_Modele_4.Text)

        Create_Save_Produits_compatible(DBBase, produits_compatible_id_5.InnerText, ddl_Produit_Compatible_Type_5.Text,
                                        Panel_Fiche_Produit_Compatible_Annee_5.Text, Panel_Fiche_Produit_Compatible_Marque_5.Text, Panel_Fiche_Produit_Compatible_Modele_5.Text)


        Save_Produits_Saison(DBBase, Prod_id, ddl_Panel_Fiche_Produit_Prod_saison.Text, ddl_Panel_Fiche_Produit_Prod_saison.Text, ddl_Panel_Fiche_Produit_Prod_saison.Text, ddl_Panel_Fiche_Produit_Prod_saison.Text)

        Dim Prod_couleur As String = ddl_Panel_Fiche_Produit_Prod_couleur.Text
        Select Case True
            Case Prod_couleur = "Choisir Couleur" And Panel_Fiche_Produit_Prod_couleur.Value = "CHOISIR COULEUR" Or Prod_couleur = "Choisir Couleur" And Panel_Fiche_Produit_Prod_couleur.Value = Nothing
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Nothing, "Prod_couleur", DBBase)
            Case Panel_Fiche_Produit_Prod_couleur.Value <> Nothing
                Prod_couleur = Panel_Fiche_Produit_Prod_couleur.Value
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_couleur, "Prod_couleur", DBBase)
            Case Prod_couleur = Nothing
            Case Prod_couleur <> Nothing
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_couleur, "Prod_couleur", DBBase)
        End Select



        Dim Prod_longueur As String = Panel_Fiche_Produit_Prod_longueur.Value
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_longueur, "Prod_longueur", DBBase)

        Dim Prod_largeur As String = Panel_Fiche_Produit_Prod_largeur.Value
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_largeur, "Prod_largeur", DBBase)

        Dim Prod_hauteur As String = Panel_Fiche_Produit_Prod_hauteur.Value
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_hauteur, "Prod_hauteur", DBBase)

        Dim Prod_poids As String = Panel_Fiche_Produit_Prod_poids.Value
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_poids, "Prod_poids", DBBase)


        Dim Prod_Marque_Financement As String = Panel_Fiche_Produit_Compatible_Marque_1.Text & "," & Panel_Fiche_Produit_Compatible_Marque_2.Text & "," & Panel_Fiche_Produit_Compatible_Marque_3.Text & "," & Panel_Fiche_Produit_Compatible_Marque_4.Text & "," & Panel_Fiche_Produit_Compatible_Marque_5.Text
        Select Case True
            Case Prod_Marque_Financement <> "Selectionner,Selectionner,Selectionner,Selectionner,Selectionner"
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_Marque_Financement, "Prod_Marque_Financement", DBBase)

                Dim Prod_Modele_Financement As String = Panel_Fiche_Produit_Compatible_Modele_1.Text & "," & Panel_Fiche_Produit_Compatible_Modele_2.Text & "," & Panel_Fiche_Produit_Compatible_Modele_3.Text & "," & Panel_Fiche_Produit_Compatible_Modele_4.Text & "," & Panel_Fiche_Produit_Compatible_Modele_5.Text
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_Modele_Financement, "Prod_Modele_Financement", DBBase)

                Dim Prod_Annee_Financement As String = Panel_Fiche_Produit_Compatible_Annee_1.Text & "," & Panel_Fiche_Produit_Compatible_Annee_2.Text & "," & Panel_Fiche_Produit_Compatible_Annee_3.Text & "," & Panel_Fiche_Produit_Compatible_Annee_4.Text & "," & Panel_Fiche_Produit_Compatible_Annee_5.Text
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_Annee_Financement, "Prod_Annee_Financement", DBBase)
        End Select


        Dim Prod_grandeur As String = ddl_Panel_Fiche_Produit_Prod_grandeur.Text
        If Prod_grandeur <> "Choisir Grandeur / Format" And Prod_grandeur <> "" Then

            stg_ = Prod_grandeur
            Prod_grandeur = stg_.Substring(stg_.Length - 6, 6)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_grandeur, "Prod_grandeur", DBBase)
        Else

            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Nothing, "Prod_grandeur", DBBase)
        End If

        Dim Prod_genre As String = ddl_Panel_Fiche_Produit_Prod_genre.Text
        Dim Prod_sexe As String = ddl_Panel_Fiche_Produit_Prod_sexe.Text
        If Prod_sexe <> "Choisir Sexe" And Prod_sexe <> "" Then
            stg_ = Prod_sexe
            Prod_sexe = stg_.Substring(stg_.Length - 6, 6)
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_sexe, "Prod_sexe", DBBase)
        Else
            Where_To_Update(DBSelection_, FieldVariable_, Variable_, Nothing, "Prod_sexe", DBBase)
        End If

        Dim Prod_desc As String = Panel_Fiche_Produit_Description.Value
        Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_desc, "Prod_desc", DBBase)


        Load_Fiche_Produits(Panel_Fiche_Produit_id_chercher.Value)

    End Sub

    Sub Save_Produits_Saison(DBBase, Prod_id, Prod_saison_hiver, Prod_saison_printemps, Prod_saison_automne, Prod_saison_ete)
        Dim stg_ As String = Prod_saison_hiver
        Dim Prod_saison_descr_FR As String = stg_.Substring(0, stg_.Length - 7)
        Dim Prod_saison As String = stg_.Substring(stg_.Length - 6, 6)

        Select Case True
            Case Prod_saison = "SAI"
                Prod_saison_hiver = Nothing
                Prod_saison_printemps = Nothing
                Prod_saison_automne = Nothing
                Prod_saison_ete = Nothing

            Case Prod_saison = "SAI001"
                Prod_saison_hiver = Nothing
                Prod_saison_printemps = Nothing
                Prod_saison_automne = Nothing
                Prod_saison_ete = "YES"

            Case Prod_saison = "SAI002"
                Prod_saison_hiver = "YES"
                Prod_saison_printemps = Nothing
                Prod_saison_automne = Nothing
                Prod_saison_ete = Nothing

            Case Prod_saison = "SAI003"
                Prod_saison_hiver = Nothing
                Prod_saison_printemps = "YES"
                Prod_saison_automne = "YES"
                Prod_saison_ete = "YES"

            Case Prod_saison = "SAI004"
                Prod_saison_hiver = "YES"
                Prod_saison_printemps = "YES"
                Prod_saison_automne = "YES"
                Prod_saison_ete = "YES"

        End Select


        Where_To_Update("produits", "Prod_id", Prod_id, Prod_saison_hiver, "Prod_saison_hiver", DBBase)
        Where_To_Update("produits", "Prod_id", Prod_id, Prod_saison_printemps, "Prod_saison_printemps", DBBase)
        Where_To_Update("produits", "Prod_id", Prod_id, Prod_saison_ete, "Prod_saison_ete", DBBase)
        Where_To_Update("produits", "Prod_id", Prod_id, Prod_saison_automne, "Prod_saison_automne", DBBase)
    End Sub
    Sub Create_Save_Produits_compatible(DBBase, produits_compatible_id, produits_compatible_Prod_Style, produits_compatible_Prod_annee, produits_compatible_Prod_marque, produits_compatible_Prod_modele)
        Select Case produits_compatible_Prod_Style
            Case "Selectionner"

            Case Else

                If produits_compatible_id = Nothing Then
                    produits_compatible_id = Find_Next_produits_compatible(connecStr, DBBase)
                End If

                Where_To_Update("produits_compatible", "produits_compatible_id", produits_compatible_id, produits_compatible_Prod_Style, "produits_compatible_Prod_Style", DBBase)
                Where_To_Update("produits_compatible", "produits_compatible_id", produits_compatible_id, produits_compatible_Prod_annee, "produits_compatible_Prod_annee", DBBase)
                Where_To_Update("produits_compatible", "produits_compatible_id", produits_compatible_id, produits_compatible_Prod_marque, "produits_compatible_Prod_marque", DBBase)
                Where_To_Update("produits_compatible", "produits_compatible_id", produits_compatible_id, produits_compatible_Prod_modele, "produits_compatible_Prod_modele", DBBase)
                Where_To_Update("produits_compatible", "produits_compatible_id", produits_compatible_id, "true", "produits_compatible_online", DBBase)
        End Select
    End Sub



    Protected Sub rpt_load_event_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim Variable_ As String = e.CommandArgument
        Dim CommandName_ As String = e.CommandName
        Dim DBSelection_ As String = "produits"
        Dim FieldVariable_ As String = "Prod_id"
        Select Case True
            Case CommandName_ = "OFFLINE"
                Dim Prod_Actif As String = GetColumnValueFromDB(DBSelection_, FieldVariable_, Variable_, "Prod_Actif", "GMBGMB")
                Select Case True
                    Case Prod_Actif = Nothing
                        Prod_Actif = "true"
                    Case Prod_Actif = "true"
                    Case Prod_Actif = "false"
                    Case Else

                End Select
                Where_To_Update(DBSelection_, FieldVariable_, Variable_, Prod_Actif, "Prod_Actif", "gmb")
            Case Else
                Panel_MontageWeb.Visible = False
                Panel_Fiche_Produit.Visible = True
                Load_Fiche_Produits(Variable_)
        End Select


    End Sub
    Public Sub ChargerDonneesEtTrier()
        ' Initialiser les valeurs par défaut des DropDownList
        Panel_Fiche_Produit_Compatible_Annee_1.Items.Clear()
        Panel_Fiche_Produit_Compatible_Annee_2.Items.Clear()
        Panel_Fiche_Produit_Compatible_Annee_3.Items.Clear()
        Panel_Fiche_Produit_Compatible_Annee_4.Items.Clear()
        Panel_Fiche_Produit_Compatible_Annee_5.Items.Clear()
        Panel_Fiche_Produit_Compatible_Marque_1.Items.Clear()
        Panel_Fiche_Produit_Compatible_Marque_2.Items.Clear()
        Panel_Fiche_Produit_Compatible_Marque_3.Items.Clear()
        Panel_Fiche_Produit_Compatible_Marque_4.Items.Clear()
        Panel_Fiche_Produit_Compatible_Marque_5.Items.Clear()
        Panel_Fiche_Produit_Compatible_Modele_1.Text = Nothing
        Panel_Fiche_Produit_Compatible_Modele_2.Text = Nothing
        Panel_Fiche_Produit_Compatible_Modele_3.Text = Nothing
        Panel_Fiche_Produit_Compatible_Modele_4.Text = Nothing
        Panel_Fiche_Produit_Compatible_Modele_5.Text = Nothing
        Panel_Fiche_Produit_Compatible_Annee_1.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Annee_2.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Annee_3.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Annee_4.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Annee_5.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Marque_1.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Marque_2.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Marque_3.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Marque_4.Items.Add("Selectionner")
        Panel_Fiche_Produit_Compatible_Marque_5.Items.Add("Selectionner")
        ' Définir la chaîne de connexion
        Dim query As String = "SELECT Unites_annee, Unites_marque, Unites_modele FROM vehicules WHERE Unites_interne = 'O'"
        connecStr = connecStr.Replace("gsi", "nor")
        Try
            ' Utiliser une connexion sécurisée
            Using connection As New MySqlConnection(connecStr)
                Using command As New MySqlCommand(query, connection)
                    connection.Open()

                    ' Lire les données
                    Using sqlReader As MySqlDataReader = command.ExecuteReader()
                        Dim annees As New List(Of Integer)()
                        Dim marques As New List(Of String)()
                        Dim modeles As New List(Of String)()

                        Dim anneeStr As Integer = 1985
                        While anneeStr < 2025
                            If Not annees.Contains(anneeStr) Then
                                annees.Add(anneeStr)
                            End If
                            anneeStr += 1
                        End While
                        ' Ajouter les années triées dans dp_annee_VE
                        For Each annee In annees
                            Panel_Fiche_Produit_Compatible_Annee_1.Items.Add(annee.ToString())
                            Panel_Fiche_Produit_Compatible_Annee_2.Items.Add(annee.ToString())
                            Panel_Fiche_Produit_Compatible_Annee_3.Items.Add(annee.ToString())
                            Panel_Fiche_Produit_Compatible_Annee_4.Items.Add(annee.ToString())
                            Panel_Fiche_Produit_Compatible_Annee_5.Items.Add(annee.ToString())
                        Next


                        While sqlReader.Read()
                            ' Ajouter les années si elles ne sont pas déjà présentes

                            ' Ajouter les marques si elles ne sont pas déjà présentes
                            Dim marque As String = sqlReader("Unites_marque").ToString()
                            If Not marques.Contains(marque) Then
                                marques.Add(marque)
                            End If

                            ' Ajouter les marques si elles ne sont pas déjà présentes
                            Dim modele As String = sqlReader("Unites_modele").ToString()
                            If Not modeles.Contains(modele) Then
                                modeles.Add(modele)
                            End If
                        End While

                        ' Trier les années et les marques
                        annees.Sort(Function(x, y) y.CompareTo(x)) ' Tri décroissant pour les années
                        marques.Sort() ' Tri alphabétique pour les marques
                        modeles.Sort() ' Tri alphabétique pour les marques



                        ' Ajouter les marques triées dans dp_marque_VE
                        For Each marque In marques
                            Panel_Fiche_Produit_Compatible_Marque_1.Items.Add(marque)
                            Panel_Fiche_Produit_Compatible_Marque_2.Items.Add(marque)
                            Panel_Fiche_Produit_Compatible_Marque_3.Items.Add(marque)
                            Panel_Fiche_Produit_Compatible_Marque_4.Items.Add(marque)
                            Panel_Fiche_Produit_Compatible_Marque_5.Items.Add(marque)
                        Next
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            Console.WriteLine("Erreur SQL : " & ex.Message)
        Catch ex As Exception
            Console.WriteLine("Erreur générale : " & ex.Message)
        End Try
    End Sub

    Protected Sub btn_Panel_MenuWeb_ServerClick(sender As Object, e As EventArgs)
        Load_Montage_Web("LOAD", "")
    End Sub

    Protected Sub btn_load_event_ItemCommand(sender As Object, e As CommandEventArgs)
        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Fidelity_id As String = Nothing,
            BondeTravail As String = Nothing,
            clientsweb_id As String = Nothing,
            LettreTravail As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, clientsweb_id, Fidelity_id, Query_Langue,
                    emailid, securitypin, semp, SCompagnie)

        Select Case True

            Case e.CommandName = "UPDATE"
                Load_Scenario_API_MYSQL(SCompagnie)
            Case e.CommandName = "EXPORT" Or e.CommandName = "IMPORT"
                Function_FTP_GSI(e.CommandName, e.CommandArgument, SCompagnie)

        End Select

        Select Case True
            Case e.CommandName = "EXPORT"
            Case e.CommandName = "IMPORT"
            Case e.CommandName = "LIST"
                Load_Montage_Web("LOAD", e.CommandArgument)
            Case Else
                Load_Montage_Web("LOAD", "")
        End Select

    End Sub
    Dim default_source As String = "C:\chemin-local\"
    Dim ftp_FSI As String = "ftp://142.112.1.133/"
    Dim ftp_FSI_User As String = "WEBFTP"
    Dim ftp_FSI_Pass As String = "Admin204$"

    Sub Function_FTP_GSI(CommandName_, CommandArgument_, SCompagnie)
        Try
            Select Case CommandName_
                Case "UPLOAD", "IMPORT"
                    Dim WebRequest_Create_Fichier_lien As String = "444791022_1010444564059739_3499050923624184337_n.png"
                    Select Case CommandArgument_
                        Case "TABLE"
                            WebRequest_Create_Fichier_lien = WebRequest_Create_Fichier_lien
                        Case "MONTAGE"
                            WebRequest_Create_Fichier_lien = WebRequest_Create_Fichier_lien
                    End Select

                    Dim WebRequest_Create_Fichier As String = ftp_FSI & WebRequest_Create_Fichier_lien
                    Dim request As FtpWebRequest = CType(WebRequest.Create(WebRequest_Create_Fichier), FtpWebRequest)
                    request.Credentials = New NetworkCredential(ftp_FSI_User, ftp_FSI_Pass)

                    Dim downloadFolder As String = GetDownloadsFolder()
                    Dim fileContents_Create_Fichier As String = downloadFolder & "\" & WebRequest_Create_Fichier_lien
                    request.Method = WebRequestMethods.Ftp.UploadFile

                    Dim fileContents() As Byte = File.ReadAllBytes(fileContents_Create_Fichier)
                    request.ContentLength = fileContents.Length

                    Using requestStream As Stream = request.GetRequestStream()
                        requestStream.Write(fileContents, 0, fileContents.Length)
                    End Using

                Case "DOWNLOAD", "EXPORT"

                    ' Obtenir le dossier de téléchargement par défaut
                    Dim WebRequest_Create_Fichier_lien As String = "2000000001.jpg"
                    Select Case CommandArgument_
                        Case "TABLE"
                            WebRequest_Create_Fichier_lien = WebRequest_Create_Fichier_lien
                        Case "MONTAGE"
                            WebRequest_Create_Fichier_lien = WebRequest_Create_Fichier_lien
                    End Select

                    Dim downloadFolder As String = GetDownloadsFolder()

                    Dim fileContents_Create_Fichier As String = downloadFolder & "\" & WebRequest_Create_Fichier_lien ' Destination locale où le fichier sera enregistré
                    Dim url As String = "https://www.seiapi.online/GoogleDrive/" & WebRequest_Create_Fichier_lien

                    Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                    request.Method = WebRequestMethods.Http.Get

                    Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                        Using responseStream As Stream = response.GetResponseStream()
                            Using fileStream As New FileStream(fileContents_Create_Fichier, FileMode.Create)
                                responseStream.CopyTo(fileStream)
                            End Using
                        End Using
                    End Using


            End Select


        Catch ex As Exception

        End Try

    End Sub

    ' Fonction pour obtenir le dossier de téléchargement par défaut de l'utilisateur
    Function GetDownloadsFolder() As String
        Return CType(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", Nothing), String)
    End Function


    Sub Load_Montage_Web_ddl()

        Dim Prod_famille_id As String = ddl_Panel_MenuWeb_Famille.Text
        If Prod_famille_id = "Choisir Famille" Then
            Prod_famille_id = ""
        End If

        Dim Prod_groupe_id As String = ddl_Panel_MenuWeb_Groupe.Text
        If Prod_groupe_id = "Choisir Groupe" And Prod_groupe_id = "Aucun Groupe" Then
            Prod_groupe_id = ""
        End If

        Dim Prod_type_id As String = ddl_Panel_MenuWeb_Type.Text
        If Prod_type_id = "Choisir Type" Then
            Prod_type_id = ""
        End If

        Dim Prod_categorie_id As String = ddl_Panel_MenuWeb_Categorie.Text
        If Prod_categorie_id = "Choisir Categorie" Then
            Prod_categorie_id = ""
        End If

        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)
            Dim stg_ As String = "Choisir Couleur"
            list_.Add(stg_)


            ddl_Panel_MenuWeb_Prod_couleur.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "gsi_color", "", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("gsi_color_fr").ToString.Trim
                Dim item As String = employe
                If Not list_2.Contains(employe) Then
                    list_2.Add(employe)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
            For Each item As String In list_
                ddl_Panel_MenuWeb_Prod_couleur.Items.Add(item)
            Next

            ddl_Panel_MenuWeb_Prod_couleur.Text = stg_
        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)

            Dim stg_ As String = "Choisir Sexe"
            list_.Add(stg_)


            ddl_Panel_MenuWeb_Prod_sexe.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'SEX'", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
            For Each item As String In list_
                ddl_Panel_MenuWeb_Prod_sexe.Items.Add(item)
            Next

            ddl_Panel_MenuWeb_Prod_sexe.Text = stg_

        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)


            Dim stg_ As String = "Choisir Saison"
            list_.Add(stg_)

            ddl_Panel_MenuWeb_Prod_saison.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'SAI'", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
            For Each item As String In list_
                ddl_Panel_MenuWeb_Prod_saison.Items.Add(item)
            Next

            ddl_Panel_MenuWeb_Prod_saison.Text = stg_

        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)

            Dim stg_ As String = "Choisir Grandeur / Format"
            list_.Add(stg_)

            ddl_Panel_MenuWeb_Prod_grandeur.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'FOR' ORDER BY Prod_groupe_id ASC", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'GRA' ORDER BY Prod_groupe_id ASC", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
            For Each item As String In list_
                ddl_Panel_MenuWeb_Prod_grandeur.Items.Add(item)
            Next

            ddl_Panel_MenuWeb_Prod_grandeur.Text = stg_


        Catch ex As Exception

        End Try
        Try
            If Prod_famille_id = "" Then

                Dim list_2 As New List(Of String)
                Dim list_ As New List(Of String)

                Dim stg_ As String = "Choisir Famille"

                ddl_Panel_MenuWeb_Famille.Items.Clear()

                Dim Get_Data_ As New DataTable
                Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'TYP' or Prod_famille_id = 'PNE' or Prod_famille_id = 'VET'", 1, 10000)
                For Each row As DataRow In Get_Data_.Rows
                    Dim employe As String = row("Prod_famille_id").ToString.Trim
                    Dim nom As String = row("Prod_Description_fr").ToString.Trim
                    Dim item As String = nom & " " & employe
                    If Not list_2.Contains(employe) Then
                        list_2.Add(employe)
                        list_.Add(item)
                    End If

                Next

                ' Trier la liste
                list_.Sort()

                ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
                For Each item As String In list_
                    ddl_Panel_MenuWeb_Famille.Items.Add(item)
                Next
                ddl_Panel_MenuWeb_Famille.Items.Add(stg_)
                ddl_Panel_MenuWeb_Famille.Text = stg_

            End If

        Catch ex As Exception

        End Try
        Try
            If Prod_groupe_id = "" And Prod_famille_id <> Nothing Then

                Dim list_2 As New List(Of String)
                Dim list_ As New List(Of String)

                Dim stg_ As String = "Choisir Groupe"

                ddl_Panel_MenuWeb_Groupe.Items.Clear()



                Dim Get_Data_ As New DataTable
                Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'TYP' or Prod_famille_id = 'PNE' or Prod_famille_id = 'VET' AND Prod_groupe_id LIKE '%" &
                                    Prod_famille_id.Substring(Prod_famille_id.Length - 3, 3) & "%'", 1, 10000)
                For Each row As DataRow In Get_Data_.Rows
                    Dim employe As String = row("Prod_groupe_id").ToString.Trim
                    Dim nom As String = row("Prod_Description_fr").ToString.Trim
                    Dim item As String = nom & " " & employe
                    If Not list_2.Contains(nom) Then
                        list_2.Add(nom)
                        list_.Add(item)
                    End If

                Next

                ' Trier la liste
                list_.Sort()
                If list_.Count > 0 Then
                    Panel_MenuWeb_Groupe.Visible = True
                    ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
                    For Each item As String In list_
                        ddl_Panel_MenuWeb_Groupe.Items.Add(item)
                    Next

                    ddl_Panel_MenuWeb_Groupe.Items.Add("Aucun Groupe")
                    ddl_Panel_MenuWeb_Groupe.Items.Add(stg_)
                    ddl_Panel_MenuWeb_Groupe.Text = stg_
                End If


            End If

        Catch ex As Exception

        End Try
        Try
            If Prod_type_id = "" And Prod_groupe_id <> Nothing And Prod_type_id = "" And Prod_groupe_id <> "Aucun Groupe" Then

                Dim list_2 As New List(Of String)
                Dim list_ As New List(Of String)

                Dim stg_ As String = "Choisir Type"

                ddl_Panel_MenuWeb_Type.Items.Clear()

                Dim Get_Data_ As New DataTable
                Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'TYP' or Prod_famille_id = 'PNE' or Prod_famille_id = 'VET' AND Prod_type_id LIKE '%" &
                                    Prod_groupe_id.Substring(Prod_famille_id.Length - 6, 6) & "%'", 1, 10000)
                For Each row As DataRow In Get_Data_.Rows
                    Dim employe As String = row("Prod_type_id").ToString.Trim
                    Dim nom As String = row("Prod_fam_style_fr").ToString.Trim
                    Dim item As String = nom & " " & employe
                    If Not list_2.Contains(nom) Then
                        list_2.Add(nom)
                        list_.Add(item)
                    End If

                Next

                ' Trier la liste
                list_.Sort()
                If list_.Count > 0 Then
                    Panel_MenuWeb_Type.Visible = True
                    ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
                    For Each item As String In list_
                        ddl_Panel_MenuWeb_Type.Items.Add(item)
                    Next

                    ddl_Panel_MenuWeb_Type.Items.Add(stg_)
                    ddl_Panel_MenuWeb_Type.Text = stg_
                End If


            End If

        Catch ex As Exception

        End Try
        Try
            If Prod_categorie_id = "" And Prod_type_id <> Nothing Then

                Dim list_2 As New List(Of String)
                Dim list_ As New List(Of String)

                Dim stg_ As String = "Choisir Categorie"

                ddl_Panel_MenuWeb_Categorie.Items.Clear()

                Dim Get_Data_ As New DataTable
                Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'TYP' or Prod_famille_id = 'PNE' or Prod_famille_id = 'VET' AND Prod_categorie_id LIKE '%" &
                                    Prod_type_id.Substring(Prod_famille_id.Length - 9, 9) & "%'", 1, 10000)
                For Each row As DataRow In Get_Data_.Rows
                    Dim employe As String = row("Prod_categorie_id").ToString.Trim
                    Dim nom As String = row("Prod_cat_style_fr").ToString.Trim
                    Dim item As String = nom & " " & employe
                    If Not list_2.Contains(nom) Then
                        list_2.Add(nom)
                        list_.Add(item)
                    End If

                Next

                ' Trier la liste
                list_.Sort()
                If list_.Count > 0 Then
                    Panel_MenuWeb_Categorie.Visible = True
                    ' Ajouter les éléments triés à ddl_Panel_MenuWeb_Prod_style
                    For Each item As String In list_
                        ddl_Panel_MenuWeb_Categorie.Items.Add(item)
                    Next

                    ddl_Panel_MenuWeb_Categorie.Items.Add(stg_)
                    ddl_Panel_MenuWeb_Categorie.Text = stg_
                End If


            End If

        Catch ex As Exception

        End Try
    End Sub
    Sub Load_Panel_Fiche_Produit_ddl()
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)
            Dim stg_ As String = "Choisir Couleur"
            list_.Add(stg_)


            ddl_Panel_Fiche_Produit_Prod_couleur.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "gsi_color", "", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("gsi_color_fr").ToString.Trim
                Dim item As String = employe
                If Not list_2.Contains(employe) Then
                    list_2.Add(employe)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_            ddl_Panel_Fiche_Produit_Prod_couleur.Items.Clear()_Prod_style
            For Each item As String In list_
                ddl_Panel_Fiche_Produit_Prod_couleur.Items.Add(item)
            Next

            ddl_Panel_Fiche_Produit_Prod_couleur.Text = stg_
        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)

            Dim stg_ As String = "Choisir Sexe"
            list_.Add(stg_)


            ddl_Panel_Fiche_Produit_Prod_sexe.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'SEX'", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_Panel_Fiche_Produit_Prod_style
            For Each item As String In list_
                ddl_Panel_Fiche_Produit_Prod_sexe.Items.Add(item)
            Next

            ddl_Panel_Fiche_Produit_Prod_sexe.Text = stg_

        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)


            Dim stg_ As String = "Choisir Saison"
            list_.Add(stg_)

            ddl_Panel_Fiche_Produit_Prod_saison.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'SAI'", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Trier la liste
            list_.Sort()

            ' Ajouter les éléments triés à ddl_Panel_Fiche_Produit_Prod_style
            For Each item As String In list_
                ddl_Panel_Fiche_Produit_Prod_saison.Items.Add(item)
            Next

            ddl_Panel_Fiche_Produit_Prod_saison.Text = stg_

        Catch ex As Exception

        End Try
        Try
            Dim list_2 As New List(Of String)
            Dim list_ As New List(Of String)

            Dim stg_ As String = "Choisir Grandeur / Format"
            list_.Add(stg_)

            ddl_Panel_Fiche_Produit_Prod_grandeur.Items.Clear()

            Dim Get_Data_ As New DataTable
            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'FOR' ORDER BY Prod_groupe_id ASC", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            Get_Data_ = GetData("", "style_catalogue", " WHERE Prod_famille_id = 'GRA' ORDER BY Prod_groupe_id ASC", 1, 10000)
            For Each row As DataRow In Get_Data_.Rows
                Dim employe As String = row("Prod_groupe_id").ToString.Trim
                Dim nom As String = row("Prod_Description_fr").ToString.Trim
                Dim item As String = nom & " " & employe
                If Not list_2.Contains(nom) Then
                    list_2.Add(nom)
                    list_.Add(item)
                End If

            Next

            ' Ajouter les éléments triés à ddl_Panel_Fiche_Produit_Prod_style
            For Each item As String In list_
                ddl_Panel_Fiche_Produit_Prod_grandeur.Items.Add(item)
            Next

            ddl_Panel_Fiche_Produit_Prod_grandeur.Text = stg_


        Catch ex As Exception

        End Try
    End Sub
    Function Load_Montage_Web_Whereline(scenario_) As String
        Dim WhereLine As String
#Region "SETUP WHERELINE"
        Dim tb_search As String = tb_Panel_MontageWeb.Value
        Dim AndLine As String = ""
        Dim Prod_style As String

        Dim Prod_famille_id As String = ddl_Panel_MenuWeb_Famille.Text
        If Prod_famille_id = "Choisir Famille" Then
            Prod_famille_id = ""
        End If

        Dim Prod_groupe_id As String = ddl_Panel_MenuWeb_Groupe.Text
        If Prod_groupe_id = "Choisir Groupe" Then
            Prod_groupe_id = ""
        End If

        Dim Prod_type_id As String = ddl_Panel_MenuWeb_Type.Text
        If Prod_type_id = "Choisir Type" Then
            Prod_type_id = ""
        End If

        Dim Prod_categorie_id As String = ddl_Panel_MenuWeb_Categorie.Text
        If Prod_categorie_id = "Choisir Categorie" Then
            Prod_categorie_id = ""
        End If


        Dim Prod_prix_1 As String = DropDownList6.Text
        If Prod_prix_1 = "Avec montant" Then
            Prod_prix_1 = ""
        End If


        Dim Prod_qte As String = DropDownList7.Text
        If Prod_qte = "Tous les produits" Then
            Prod_qte = ""
        End If



#End Region



        Select Case True
            Case scenario_ = "SCENARIO_1"
                AndLine = " AND Prod_groupe_id = '' or Prod_groupe_id is null"
                Panel_MenuWeb_Famille.Visible = False
            Case scenario_ = "SCENARIO_3"
                AndLine = " AND Prod_prix_1 = 0 or Prod_prix_1 < 0"
                Panel_MenuWeb_Famille.Visible = False

            Case scenario_ = "SCENARIO_2"
                Panel_MenuWeb_Famille.Visible = True

                AndLine = " AND Prod_photo_url1 LIKE '%A VENIR%' or Prod_photo_url1 is null"

                If Prod_categorie_id <> "" Then
                    AndLine += " AND Prod_categorie_id = '" & Prod_categorie_id.Substring(Prod_categorie_id.Length - 12, 12) & "'"
                Else
                    If Prod_type_id <> "" Then
                        AndLine += " AND Prod_type_id = '" & Prod_type_id.Substring(Prod_type_id.Length - 9, 9) & "'"
                    Else
                        If Prod_groupe_id <> "" Then
                            AndLine += " AND Prod_groupe_id = '" & Prod_groupe_id.Substring(Prod_groupe_id.Length - 6, 6) & "'"
                        Else
                            If Prod_famille_id <> "" Then
                                AndLine += " AND Prod_famille_id = '" & Prod_famille_id.Substring(Prod_famille_id.Length - 3, 3) & "'"
                            Else

                            End If
                        End If
                    End If
                End If
            Case scenario_ = Nothing
                Panel_MenuWeb_Famille.Visible = True
                If Prod_categorie_id <> "" Then
                    AndLine = " AND Prod_categorie_id = '" & Prod_categorie_id.Substring(Prod_categorie_id.Length - 12, 12) & "'"
                Else
                    If Prod_type_id <> "" Then
                        AndLine = " AND Prod_type_id = '" & Prod_type_id.Substring(Prod_type_id.Length - 9, 9) & "'"
                    Else
                        If Prod_groupe_id <> "" Then
                            If Prod_groupe_id = "Aucun Groupe" Then
                                AndLine = " AND Prod_groupe_id = '' or Prod_groupe_id is null"
                            Else
                                AndLine = " AND Prod_groupe_id = '" & Prod_groupe_id.Substring(Prod_groupe_id.Length - 6, 6) & "'"
                            End If
                        Else
                            If Prod_famille_id <> "" Then
                                AndLine = " AND Prod_famille_id = '" & Prod_famille_id.Substring(Prod_famille_id.Length - 3, 3) & "'"
                            Else

                            End If
                        End If
                    End If
                End If
        End Select

        Select Case True
            Case Prod_prix_1 = Nothing
                AndLine += " AND Prod_prix_1 > 0"
            Case Prod_prix_1 = "A zero"
                AndLine += " AND Prod_prix_1 < 0 or Prod_prix_1 = 0"
        End Select
        Select Case True
            Case Prod_qte = Nothing
            Case Prod_qte = "En inventaire"
                AndLine += " AND Prod_qte > 0"
            Case Prod_qte = "En commande"
                AndLine += " AND Prod_qte < 0"
            Case Prod_qte = "A commander"
                AndLine += " AND Prod_qte = 0"
        End Select



        Select Case True
            Case tb_search <> Nothing
                AndLine = " AND Prod_Actif = 'true'" & Prod_style

                WhereLine = " WHERE Prod_id LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_groupe_id_descr_FR LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_groupe_id_descr_EN LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_style LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_desc LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_desca LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_groupe_id LIKE '%" & tb_search & "%'" & AndLine & " OR " &
                                                        "Prod_fournisseur1 LIKE '%" & tb_search & "%'" & AndLine

            Case Else
                WhereLine = " where Prod_Actif = 'true'" & AndLine
        End Select

        Return WhereLine
    End Function
    Function Load_Montage_Web_recordpage() As Integer
        Dim recordsPerPage As Integer
        Try
            Select Case True
                Case ddl_Panel_MenuWeb_Page_View.Text = "Pas de limite"
                    recordsPerPage = 10000000
                Case Else
                    recordsPerPage = Val(ddl_Panel_MenuWeb_Page_View.Text)
            End Select
        Catch ex As Exception

        End Try

        Return recordsPerPage
    End Function
    Function Load_Montage_Web_selectpage(scenario_, tot_, recordsPerPage) As Integer
        Dim select_page As Integer
        Try
            Dim max_page As Double = Val(tot_) / recordsPerPage
            If max_page < 1 Then
                max_page = 1
            End If

            Select Case scenario_
                Case "LOAD", "REFRESH"
                    select_page = select_page
                Case "PREV"
                    select_page = Val(page_Panel_MontageWeb.InnerText) - 1
                Case "NEXT"
                    select_page = Val(page_Panel_MontageWeb.InnerText) + 1
            End Select
            Select Case True
                Case max_page < select_page
                    select_page = max_page
                Case max_page = select_page
                    select_page = max_page
                Case select_page <= 0
                    select_page = 1
                Case Else
                    select_page = select_page
            End Select
        Catch ex As Exception

        End Try
        Return select_page
    End Function
    Sub Load_Montage_Web(scenario_, event_)
        Try
            Dim Database As String = "produits"
            Dim WhereLine As String = Load_Montage_Web_Whereline(event_)
            Dim tot_ As Integer = Process_Count_Mysql("Load_Montage_Web", Database, WhereLine).ToString
            Status_Panel_MontageWeb.InnerText = tot_.ToString
            Dim recordsPerPage As Double = Load_Montage_Web_recordpage()
            Dim select_page As Double = Load_Montage_Web_selectpage(scenario_, tot_, recordsPerPage)
            page_Panel_MontageWeb.InnerText = select_page.ToString
            Dim clients_Data As DataTable = GetData("Load_Montage_Web", Database, WhereLine, select_page, recordsPerPage)
            ViewState("dt") = clients_Data
            Repeater_Panel_MontageWeb.DataSource = clients_Data
            Repeater_Panel_MontageWeb.DataBind()
        Catch ex As Exception
        End Try
        If scenario_ = "LOAD" Then
            Load_Montage_Web_ddl()
        End If
    End Sub


    Protected Sub btn_previous_Panel_MontageWeb_ServerClick(sender As Object, e As EventArgs)
        Load_Montage_Web("PREV", "")
    End Sub

    Protected Sub btn_next_Panel_MontageWeb_ServerClick(sender As Object, e As EventArgs)
        Load_Montage_Web("NEXT", "")
    End Sub



    Sub Load_EMP_Scenario(ini_Scenario_, semp, SCompagnie)

    End Sub

    Sub Function_Entree_BT(BondeTravail, LettreTravail, semp)
        Dim Scommantairevisible, SCommentaireinv, Setat As String
        Try
            If Badge_Status_WAIT.InnerText <> "Bon de travail en attente" Then
                Dim scode_2 As String = (Badge_Status_WAIT.InnerText).PadRight(10)
                Dim stg_2 = punch(semp, scode_2, Scommantairevisible, SCommentaireinv, Setat)
                If stg_2 <> Nothing Then
                    Select Case True
                        Case stg_2.Contains("Vous n'avez pas entrez votre temps de travail")
                            Panel_Entrerdestemps.Visible = True
                            LblmessagepucnhBT.InnerText = stg_2
                        Case stg_2.Contains("Impossible de reserver le vieux")
                            stg_2 = stg_2.Replace("Impossible de reserver le vieux", "")
                            stg_2 = stg_2.Replace("bon de travail", "")
                            stg_2 = stg_2.Trim

                            scode_2 = stg_2.PadRight(10)
                            Dim stg_3 = punch(semp, scode_2, Scommantairevisible, SCommentaireinv, Setat)
                    End Select
                End If
            End If
        Catch ex As Exception

        End Try
        Try
            If BondeTravail = Nothing Then
                BondeTravail = SortirJob_IDBondetravail.Value
            End If
            Load_Bon_Travail_Info(BondeTravail, LettreTravail)
            'semp = btn_EntreSortie_employe_id.Text
            Dim scode As String = BondeTravail & "-" & LettreTravail
            Dim stg_ = punch(semp, scode, Scommantairevisible, SCommentaireinv, Setat)

            Lbletatemploye.Text = stg_
            If stg_ <> Nothing Then
                Select Case True
                    Case stg_ = "Ce bon de travail contient une note finale" Or
                        stg_ = "Vous n'avez pas entrez votre temps de travail" Or
                        stg_ = "Vous avez du temps de travail pas fermer" Or
                        stg_.Contains("Impossible de reserver le vieux")
                        'LblmessagepucnhBT.InnerText = stg_
                        Panel_Entrerdestemps.Visible = True
                        Panel_Menu_Taches_Online_1.Visible = True
                        Panel_Menu_Taches.Visible = True
                        Panel_TreeViewBtemploye_M.Visible = True
                        Exit Sub
                End Select
            End If
        Catch ex As Exception

        End Try

        Try
            Response.Redirect("GroupeSEIInc.aspx", True)
        Catch ex As ThreadAbortException
        Catch ex As Exception

        End Try
        'affiche_Update_Info(Nothing, Nothing, semp)
        'Case "btn_EntreedeJob_ServerClick"

        'Try
        '    'SortirJob_IDBondetravail.Value = SortirJob_IDBondetravail.Value.ToUpper
        '    'Lblbt.Text = SortirJob_IDBondetravail.Value.ToUpper
        '    'Lbletatemploye.Text = ""
        '    If ssortie <> "" Then
        '        LblmessagepucnhBT.InnerText = "Vous devez puncher l'employé"
        '        Exit Sub
        '    End If

        '    If SortirJob_IDBondetravail.Value.Trim = "" And Lblbt.InnerText.Trim <> "" Then
        '        SortirJob_IDBondetravail.Value = Lblbt.InnerText
        '    End If
        '    If SortirJob_IDBondetravail.Value.Trim <> "" Then
        '        LblmessagepucnhBT.InnerText = ""
        '        'If TxtBoxnonvisble Is Nothing Then TxtBoxnonvisble.Text = " "
        '        'If TxtBoxvisble Is Nothing Then TxtBoxvisble.Text = " "

        '        Dim Scommentairevisible As String = Comment_Visible.InnerText.Trim


        '        Dim ival As Integer = InStr(Scommentairevisible, vbCrLf)
        '        If ival <> 0 Then Scommentairevisible = Replace(Comment_Visible.InnerText.Trim, vbCrLf, " ")
        '        Dim Scommentairenonvisible As String = Comment_NonVisible.InnerText.Trim
        '        ival = InStr(Scommentairenonvisible, vbCrLf)
        '        If ival <> 0 Then Scommentairenonvisible = Replace(Comment_NonVisible.InnerText.Trim, vbCrLf, " ")
        '        ' punch(TxtBT.Text.Trim.PadRight(10) & Scommentairevisible.PadRight(200) & Scommentairenonvisible.PadRight(200))

        '        Dim scode As String = SortirJob_IDBondetravail.Value.Trim.PadRight(10) &
        '                              Scommentairevisible.PadRight(125).ToString &
        '                              Scommentairenonvisible.PadRight(125).ToString

        '        Dim stg_ As String = punch(semp, scode)
        '        BondeTravail = SortirJob_IDBondetravail.Value
        '        LettreTravail = BondeTravail.Substring(BondeTravail.Length - 1, 1)
        '        Select Case stg_
        '            Case "Vous avez du temps de travail pas fermer"
        '                Panel_Entrerdestemps.Visible = True
        '                Offline_Panel.Visible = False

        '                Panel_Parametre_Online_SubMenu.Visible = True
        '                Panel_Parametre_Online_Menu.Visible = True
        '                btn_Panel_Entrerdestemps.Visible = True
        '                btn_Panel_Entrerdespieces.Visible = True
        '                btn_Panel_Entrerdestravails.Visible = True

        '                Panel_Entrerdespieces.Visible = True
        '                Panel_Entrerdestravails.Visible = True
        '            Case Else
        '                afficheinfo_Bontravail_emp_Scenario_3(BondeTravail, LettreTravail, semp)
        '        End Select
        '        Lbletatemploye.Text = stg_

        '    Else
        '        LblmessagepucnhBT.InnerText = "le No de bon de travail est vide !"
        '    End If


        'Catch ex As Exception

        'End Try


    End Sub


    Protected Sub btn_Panel_Repeater_wo_complet_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        afficheinfo_all_wo("btn_Panel_Repeater_wo_complet_ServerClick", Nothing, "FLASH", "ALL", SCompagnie)

    End Sub
    Protected Sub btn_Panel_TreeView_kt_modal_allWO_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        afficheinfo_all_wo("btn_Panel_TreeView_kt_modal_allWO_ServerClick", Nothing, "FLASH", "ALL", SCompagnie)
    End Sub
    Protected Sub btn_Panel_Repeater_Metronic_allWO_reset_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        afficheinfo_all_wo("btn_Panel_Repeater_Metronic_allWO_reset_ServerClick", Nothing, "FLASH", "ALL", SCompagnie)
    End Sub
    Protected Sub btn_Panel_Repeater_Metronic_Btemploye_M_cancel_ServerClick(sender As Object, e As EventArgs)

        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        TextBox_Panel_Repeater_Metronic_Btemploye_M.Text = ""

        Dim ires As Integer = 0
        ires = InStr(semp, "(")
        If ires <> 0 Then
            semp = Mid(semp, 1, ires - 2)
        End If
        Dim Sapi As String = afficher_BT_Jobs_Apogee(semp)

        afficherBTemployeetjob("EMPBT", Sapi, semp, SCompagnie)

    End Sub
    Protected Sub Repeater_wo_complet_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim BondeTravail As String = e.CommandArgument.ToString.Trim
        Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")
        'Dim ProductToShow_ As Integer = Process_Count_Mysql("bondetravailpiece", " WHERE FCompagnie = '" &  & "' AND Entry_dd_statut_id = 'OUI' AND Entry_dd_feature = '" & Clients_email & "'")
        lb_Panel_wo_complet.InnerText = BondeTravail & " { " & ProductToShow_.ToString & " Taches }"

        Panel_Repeater_wo_complet.Visible = False
        Panel_Repeater_wo_complet_Load_Jobs.Visible = True

        Try
            Dim WhereLine As String = " WHERE BondeTravail = '" & BondeTravail & "' AND SCompagnie = '" & SCompagnie & "' ORDER BY FLineNumber ASC"
            Dim clients_Data As DataTable = GetData("Repeater_wo_complet_ItemCommand", "bondetravailjobs", WhereLine, 1, 1000000)

            ViewState("dt") = clients_Data
            Repeater_wo_complet_Load_Jobs.DataSource = clients_Data
            Repeater_wo_complet_Load_Jobs.DataBind()
        Catch ex As Exception

        End Try

        Try
            Dim WhereLine As String = " WHERE BondeTravail LIKE '%" & BondeTravail & "%' AND SCompagnie = '" & SCompagnie & "' ORDER BY FLineNumber ASC"
            Dim clients_Data As DataTable = GetData("Repeater_wo_complet_ItemCommand", "bondetravailpiece", WhereLine, 1, 1000000)
            ViewState("dt") = clients_Data
            Repeater_wo_complet_Load_Jobs_Parts.DataSource = clients_Data
            Repeater_wo_complet_Load_Jobs_Parts.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btn_Panel_Repeater_Metronic_Btemploye_M_Jobs_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btn_Panel_Repeater_Metronic_Btemploye_M_Jobs_back_ServerClick(sender As Object, e As EventArgs)
        Panel_Repeater_Metronic_Btemploye_M_Jobs.Visible = False
        Panel_Repeater_Metronic_Btemploye_M.Visible = True
    End Sub



    Protected Sub btn_Panel_Repeater_Metronic_Btemploye_M_ServerClick(sender As Object, e As EventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)

        Dim ires As Integer = 0
        ires = InStr(semp, "(")
        If ires <> 0 Then
            semp = Mid(semp, 1, ires - 2)
        End If
        Dim Sapi As String = afficher_BT_Jobs_Apogee(semp)

        afficherBTemployeetjob("EMPBT", Sapi, semp, SCompagnie)
    End Sub

    Protected Sub Repeater_Metronic_Entry_Btemploye_M_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim semp As String = Nothing
        Dim Bondetravail As String = e.CommandArgument.Trim
        Dim LettreTravail As String = e.CommandName.Trim
        Client_Cookie("LOAD", "semp", semp)
        Start_Process(stream_api)
        Reset_Panel()
        Function_Entree_BT(Bondetravail, LettreTravail, semp)
    End Sub

    Protected Sub Repeater_Metronic_Btemploye_M_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim BondeTravail As String = e.CommandArgument.ToString.Trim
        Dim CommandName As String = e.CommandName.ToString.Trim
        Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")

        Select Case True
            Case CommandName = "PRINT"
                MainAsync(BondeTravail).Wait()

            Case Else
                lb_Panel_TreeView_kt_modal_allWO.InnerText = BondeTravail & " { " & ProductToShow_.ToString & " Taches }"

                Panel_Repeater_Metronic_Btemploye_M.Visible = False
                Panel_Repeater_Metronic_Btemploye_M_Jobs.Visible = True

                Try
                    Dim WhereLine As String = Nothing
                    Dim clients_Data As DataTable = GetData("Repeater_Metronic_allWO_ItemCommand", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "' AND SCompagnie = '" & SCompagnie & "'AND employe LIKE '%" & semp & "%' ORDER BY FLineNumber ASC", 1, 1000000)

                    ViewState("dt") = clients_Data
                    Repeater_Metronic_Btemploye_MLoad_Jobs.DataSource = clients_Data
                    Repeater_Metronic_Btemploye_MLoad_Jobs.DataBind()
                Catch ex As Exception

                End Try

                Try
                    Dim WhereLine As String = Nothing
                    Dim clients_Data As DataTable = GetData("Repeater_Metronic_allWO_ItemCommand", "bondetravailpiece", " WHERE BondeTravail LIKE '%" & BondeTravail & "%' AND SCompagnie = '" & SCompagnie & "'AND employe LIKE '%" & semp & "%' ORDER BY FLineNumber ASC", 1, 1000000)
                    ViewState("dt") = clients_Data
                    Repeater_Metronic_Btemploye_M_Load_Parts.DataSource = clients_Data
                    Repeater_Metronic_Btemploye_M_Load_Parts.DataBind()
                Catch ex As Exception

                End Try
        End Select

        'Dim ProductToShow_ As Integer = Process_Count_Mysql("bondetravailpiece", " WHERE FCompagnie = '" &  & "' AND Entry_dd_statut_id = 'OUI' AND Entry_dd_feature = '" & Clients_email & "'")


    End Sub

    Protected Sub Repeater_Metronic_allWO_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim Clients_id_ As String = Nothing, Clients_web As String = Nothing, Fidelity_id As String = Nothing, Query_Langue As String = Nothing, Clients_email As String = Nothing, securitypin As String = Nothing, semp As String = Nothing, SCompagnie As String = Nothing
        Load_Cookie("LOAD", Clients_id_, Clients_web, Fidelity_id, Query_Langue, Clients_email, securitypin, semp, SCompagnie)
        Dim BondeTravail As String = e.CommandArgument.ToString.Trim
        Dim CommandName As String = e.CommandName.ToString.Trim
        Dim ProductToShow_ As Integer = Process_Count_Mysql("", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "'")

        Select Case True
            Case CommandName = "PRINT"
                MainAsync(BondeTravail).Wait()

            Case Else
                lb_Panel_TreeView_kt_modal_allWO.InnerText = BondeTravail & " { " & ProductToShow_.ToString & " Taches }"

                Panel_Repeater_Metronic_allWO.Visible = False
                Panel_Repeater_Metronic_allWO_Load_Jobs.Visible = True

                Try
                    Dim WhereLine As String = Nothing
                    Dim clients_Data As DataTable = GetData("Repeater_Metronic_allWO_ItemCommand", "bondetravailjobs", " WHERE BondeTravail = '" & BondeTravail & "' AND SCompagnie = '" & SCompagnie & "' ORDER BY FLineNumber ASC", 1, 1000000)

                    ViewState("dt") = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataSource = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataBind()
                Catch ex As Exception

                End Try

                Try
                    Dim WhereLine As String = Nothing
                    Dim clients_Data As DataTable = GetData("Repeater_Metronic_allWO_ItemCommand", "bondetravailpiece", " WHERE BondeTravail LIKE '%" & BondeTravail & "%' AND SCompagnie = '" & SCompagnie & "' ORDER BY FLineNumber ASC", 1, 1000000)
                    ViewState("dt") = clients_Data
                    Repeater_Metronic_allWO_Load_Parts.DataSource = clients_Data
                    Repeater_Metronic_allWO_Load_Parts.DataBind()
                Catch ex As Exception

                End Try
        End Select

        'Dim ProductToShow_ As Integer = Process_Count_Mysql("bondetravailpiece", " WHERE FCompagnie = '" &  & "' AND Entry_dd_statut_id = 'OUI' AND Entry_dd_feature = '" & Clients_email & "'")


    End Sub

    Protected Sub btn_Panel_Repeater_wo_complet_back_ServerClick(sender As Object, e As EventArgs)
        lb_Panel_wo_complet.InnerText = "Tous les bon de travails"
        Panel_Repeater_wo_complet.Visible = True
        Panel_Repeater_wo_complet_Load_Jobs.Visible = False
    End Sub
    Protected Sub btn_Panel_Repeater_Metronic_allWO_Load_Jobs_back_ServerClick(sender As Object, e As EventArgs)
        lb_Panel_TreeView_kt_modal_allWO.InnerText = "Tous les bon de travails"
        Panel_Repeater_Metronic_allWO.Visible = True
        Panel_Repeater_Metronic_allWO_Load_Jobs.Visible = False
    End Sub


    Protected Sub btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick")

    End Sub



    Protected Sub btn_Panel_details_cli_detail_Fiche_ServerClick(sender As Object, e As EventArgs)
        Panel_details_cli_detail_Histoire.Visible = True
        Panel_details_cli_detail_Fiche.Visible = False
    End Sub

    Protected Sub btn_Panel_details_cli_detail_Histoire_ServerClick(sender As Object, e As EventArgs)
        Panel_details_cli_detail_Histoire.Visible = False
        Panel_details_cli_detail_Fiche.Visible = True
    End Sub


    Protected Sub btn_Panel_details_cli_detail_Histoire_Search_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btn_Panel_TreeViewBtemploye_M_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_TreeViewBtemploye_M_ServerClick")
    End Sub

    Protected Sub btn_Panel_TreeViewBtemploye_M_reset_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_TreeViewBtemploye_M_reset_ServerClick")
    End Sub

    Protected Sub btn_Panel_PreFacture_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_PreFacture_ServerClick")

    End Sub

    Protected Sub btn_Liste_Commande_Pieces_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btn_Liste_Soumission_Pieces_ServerClick(sender As Object, e As EventArgs)

    End Sub




#End Region

    Sub Load_Scenario_PAGE_LOAD(Scenario_, Query_Langue, clientsweb_id, Clients_id_, Fidelity_id, semp, SCompagnie)
        Dim Clients_addr_1 As String = Nothing
        Dim Clients_comp As String = Nothing
        Show_Online_Client(Clients_id_, Fidelity_id, Clients_comp, Clients_addr_1)
        Select Case True
            Case Clients_addr_1 = Nothing And Clients_comp <> Nothing
                Main_Level_2()
            Case Clients_comp = Nothing
                Main_Level_2()
            Case Else
                Dim clientsweb_scompagnie_roles As String = GetColumnValueFromDB("clientsweb", "clientsweb_id", clientsweb_id, "clientsweb_scompagnie_roles", "GSIGSI")

                Select Case clientsweb_scompagnie_roles
                    Case "BT"


                    Case "MWEB"
                        Panel_Parametre_Online_Menu.Visible = False
                        Panel_Parametre_Online_SubMenu.Visible = False
                        Panel_MontageWeb.Visible = True
                        Load_Montage_Web("LOAD", "")
                        Exit Sub
                    Case "ALL"
                        If semp = Nothing Then
                            semp = "DBL"
                            Client_Cookie("WRITE", "semp", semp)
                        End If
                        If SCompagnie = Nothing Then
                            SCompagnie = "999999"
                            'SCompagnie = "COBWEB"
                            Client_Cookie("WRITE", "SCompagnie", SCompagnie)
                        End If
                        If clientsweb_id = Nothing Then
                            clientsweb_id = GetColumnValueFromDB("clients", "Clients_id", Clients_id_, "Clients_noclient", "GSIGSI")
                            Client_Cookie("WRITE", "Clients_web", clientsweb_id)
                        End If

                End Select

                Select Case True
                    Case SCompagnie = ""
                        Load_Scenario_API_MYSQL(SCompagnie)
                End Select
                Load_Scenario_API_ONLINE(semp, SCompagnie)
        End Select
    End Sub
    Sub Load_Scenario_OFFLINE(Scenario_, Clients_id_, Clients_web, Fidelity_id, semp, SCompagnie)
        Load_Scenario_Panel_Menu_Offline()

        Select Case Scenario_
            Case "btn_Panel_Menu_Taches_Offline_1_Menu_ServerClick"
            Case "btn_Panel_MenuWeb_Montage_Web_ServerClick", "btn_Panel_Montage_ServerClick"
                Panel_MontageWeb.Visible = True
                Load_Montage_Web("LOAD", "")
            Case "btn_Panel_Montage_WEN_ServerClick"
                Panel_MontageWeb.Visible = True
            Case "btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick"
                Try
                    Dim BondeTravail As String = lb_Panel_wo_complet.InnerText
                    BondeTravail = BondeTravail.Substring(0, 5)
                    Dim WhereLine As String = Nothing
                    Dim tb_search As String = tb_Panel_Repeater_wo_complet.Text
                    Dim clients_Data As DataTable = GetData("Load_Scenario_OFFLINE", "bondetravailjobs", " WHERE " &
                                                            "BondeTravail = '" & BondeTravail & "' AND FDescription LIKE '%" & tb_search & "%' OR " &
                                                             "BondeTravail = '" & BondeTravail & "' AND FUnite LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND Datedu LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND BondeTravail LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND employe LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND FEtat LIKE '%" & tb_search & "%'", 1, 1000000)
                    ViewState("dt") = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataSource = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataBind()

                    wo_complet.Visible = True
                    Panel_Repeater_wo_complet_Load_Jobs.Visible = True
                Catch ex As Exception

                End Try

            Case "LOG_OFF"
                Load_Cookie("WRITE", "", "", "", "", "", "", "", "")
                Try
                    Response.Redirect("GroupeSEIInc.aspx", True)
                Catch ex As ThreadAbortException
                Catch ex As Exception

                End Try

            Case "btn_offline_panel_AllJobs_ServerClick"
                afficher_BT_Jobs(Scenario_, "", "", SCompagnie, "")
                wo_complet.Visible = True


            Case "btn_offline_panel_Jobs_ServerClick"
                afficher_BT_Jobs(Scenario_, "", "", SCompagnie, "")
                wo_actuel.Visible = True



            Case "btn_offline_panel_Agenda_ServerClick"
                afficheinfo_all_emp(semp, SCompagnie)
                Offline_agenda_actuel.Visible = True
                Plage_Horaire_Offline.InnerText = GetWeekString()
        End Select
    End Sub
    Sub Load_Scenario_ONLIGNE(Scenario_, Clients_id_, Fidelity_id, semp, ssortie, SCompagnie)
        Select Case Scenario_
            Case "btn_wo_actuel_ServerClick"
                afficher_BT_Jobs(Scenario_, "", "", SCompagnie, "")
                Panel_TreeViewBtemploye_M.Visible = True

            Case "btn_BT_en_cours_employe_complet_ServerClick1"
                'afficheinfo_Recherche()
                afficher_BT_Jobs(Scenario_, "", "", SCompagnie, "")
                Panel_TreeView_kt_modal_allWO.Visible = True
                Panel_Repeater_Metronic_allWO.Visible = True

            Case "Btn_val_client_ServerClick"
                Getretourneclientfixe_(TextClientCreation.Value, "Btn_val_client_ServerClick", SCompagnie)





            Case "btn_Panel_Detail_ServerClick", "btn_Agenda_Portail_refresh_ServerClick"
                agenda_actuel.Visible = True
                afficheinfo_all_emp(semp, SCompagnie)
                Plage_Horaire.InnerText = GetWeekString()

            Case "btn_Panel_CreationBT_ServerClick"
                Panel_CreationBT.Visible = True

            Case "btn_Panel_CreationJOBS_ServerClick"
                Panel_CreationJOBS.Visible = True

            Case "btn_Panel_Entrerdestemps_ServerClick"
                Panel_Entrerdestemps.Visible = True
                Panel_CreationJOBS.Visible = True
                Panel_CreationBT.Visible = True

            Case "btn_Panel_CreationBT_M_ServerClick"
                Panel_CreationBT.Visible = True


        End Select
        Load_Scenario_Panel_Menu_Online()

    End Sub
    Sub Load_Scenario_ONLIGNE_WORKING(Scenario_, Clients_id_, BondeTravail, LettreTravail, FUnite, FClient, Fidelity_id, semp, ssortie, SCompagnie)
        Select Case Scenario_
            Case "Repeaterrechercheproduit_ItemCommand"
                Panel_Entrerdespieces.Visible = True
            Case "btn_recherche_produit_back_ServerClick"
                Panel_Entrerdespieces.Visible = True
            Case "btn_Panel_Entrerdespieces_Search_ServerClick"
                recherche_produit.Visible = True
            Case "btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick"
                Try
                    BondeTravail = lb_Panel_TreeView_kt_modal_allWO.InnerText
                    BondeTravail = BondeTravail.Substring(0, 5)
                    Dim WhereLine As String = Nothing
                    Dim tb_search As String = tb_Panel_Repeater_Metronic_allWO_Load_Jobs.Text
                    Dim clients_Data As DataTable = GetData("Load_Scenario_ONLIGNE_WORKING", "bondetravailjobs", " WHERE " &
                                                            "BondeTravail = '" & BondeTravail & "' AND FDescription LIKE '%" & tb_search & "%' OR " &
                                                             "BondeTravail = '" & BondeTravail & "' AND FUnite LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND Datedu LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND BondeTravail LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND employe LIKE '%" & tb_search & "%' OR " &
                                                            "BondeTravail = '" & BondeTravail & "' AND FEtat LIKE '%" & tb_search & "%'", 1, 1000000)
                    ViewState("dt") = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataSource = clients_Data
                    Repeater_Metronic_allWO_Load_Jobs.DataBind()

                    Panel_TreeView_kt_modal_allWO.Visible = True
                    Panel_Repeater_Metronic_allWO_Load_Jobs.Visible = True
                Catch ex As Exception

                End Try

            Case "btn_SortirdeJob_ServerClick", "SortirJob_IDBondetravail_Q2_Break_ServerClick", "SortirJob_IDBondetravail_Q2_Close_ServerClick", "SortirJob_IDBondetravail_Q2_Continue_ServerClick", "btn_Panel_PunchInOut_ServerClick"
                Load_Scenario_SortirdeJob(Scenario_, BondeTravail, LettreTravail, semp, ssortie, SCompagnie)




            Case "btn_Panel_Entrerdespieces_ServerClick"
                Panel_Entrerdespieces.Visible = True
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)

            Case "btn_Panel_details_emp_2_ServerClick", "btn_Panel_details_veh_detail_historique_jobs_ServerClick"
                'Panel_Entrerdestemps.Visible = True
                afficheinfo_veh(FUnite, SCompagnie)
                Panel_details_veh_detail.Visible = True
                Select Case Scenario_
                    Case "btn_Panel_details_veh_detail_historique_jobs_ServerClick"
                        Panel_details_veh_detail_historique_jobs.Visible = True
                End Select

            Case "btn_Panel_details_BonTravails_ServerClick", "btn_Panel_Bontravail_ServerClick"
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                Panel_Taches_Entrerdespieces.Visible = True
                Panel_Taches_Entrerdestravails.Visible = True

            Case "Filter_Letter_Parts"
                LettreTravail = dp_Filter_Letter_Parts.Text

                If LettreTravail = "Tous les jobs" Then
                    LettreTravail = "ALL"
                End If
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                Panel_Entrerdespieces.Visible = True

            Case "Filter_Letter_Tasks"

                LettreTravail = dp_Filter_Letter_Tasks.Text
                If LettreTravail = "Tous les jobs" Then
                    LettreTravail = "ALL"
                End If
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                Panel_Entrerdestravails.Visible = True

            Case "btn_Panel_Entrerdespieces_M_ServerClick"
                Panel_Entrerdespieces.Visible = True
                afficheinfo_Bontravail_parts("Panel_Entrerdespieces", BondeTravail, LettreTravail, SCompagnie, FUnite, FClient)

            Case "btn_Panel_Entrerdestravails_M_ServerClick"
                Panel_Entrerdestravails.Visible = True
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)

            Case "btn_Panel_FicheVehicules_ServerClick"
                Panel_Fiche_Vehicules.Visible = True

            Case "btn_Panel_FicheProduits_ServerClick"
                Panel_Fiche_Produit.Visible = True

            Case "btn_wo_complet_ServerClick"
                'POUR AFFICHER TOUS LES BON DE TRAVAILS SANS INTERACTION
                wo_complet.Visible = True
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                    'afficher_BT_Jobs("BT", BondeTravail, LettreTravail)
                    'afficheinfo_Bontravail_emp(semp)
            Case "Panel_details_emp_3_ServerClick", "btn_Panel_details_emp_3_ServerClick",
                 "btn_Panel_PreFacture_ServerClick"
                Getretourneclientfixe_(FClient, Scenario_, SCompagnie)
                'Panel_Entrerdestemps.Visible = True

                Select Case Scenario_
                    Case "Panel_details_emp_3_ServerClick", "btn_Panel_details_emp_3_ServerClick"
                        Panel_details_cli_detail.Visible = True
                    Case "btn_Panel_PreFacture_ServerClick"
                        Panel_PreFacture.Visible = True
                        Panel_Customer.Visible = True
                End Select


            Case "btn_Panel_Entrerdestravails_ServerClick"
                afficher_BT_Jobs(Scenario_, BondeTravail, LettreTravail, SCompagnie, FUnite)
                Panel_Entrerdestravails.Visible = True
        End Select

        Load_Scenario_Panel_Menu_Online()

    End Sub
    Sub Load_Scenario_API_MYSQL(SCompagnie)
        GetAPIStringAsync_Scenario(url_COB, "Getretournetypesiteweb", Slienpath, SCompagnie)
        GetAPIStringAsync_Scenario(url_COB, "Getretourneclientsiteweb", Slienpath, SCompagnie)
        GetAPIStringAsync_Scenario(url_COB, "Getretourneproduits", Slienpath, SCompagnie)
    End Sub
    Sub Load_Scenario_API_ONLINE(semp, SCompagnie)
        afficheinfo_all_emp(semp, SCompagnie)
        Load_Getretournerequestemploye(semp)
        affiche_Update_Info(semp, SCompagnie)
    End Sub
    Sub Load_Scenario_EXIT()
        Load_Cookie("WRITE", "", "", "", "", "", "", "", "")
        Reset_All()
        'Flex_logo.Visible = True
        Offline_Panel.Visible = True
        Panel_Choix_Email.Visible = True


        Panel_Parametre_Online_Menu.Visible = False

        Dim Clients_web_step1 As String
        Load_UI_Cookie("LOAD", "Clients_web_step1", Clients_web_step1)
        If Clients_web_step1 <> Nothing Then
            tb_email.Value = Clients_web_step1
            check_Clients_web_step1.Checked = True
        End If
    End Sub
    Sub Load_Scenario(Scenario_)
#Region "LOAD"
        Dim Clients_id_ As String = Nothing,
            semp As String = Nothing,
            Fidelity_id As String = Nothing,
            BondeTravail As String = Nothing,
            clientsweb_id As String = Nothing,
            LettreTravail As String = Nothing,
            Query_Langue As String = Nothing,
            SCompagnie As String = Nothing,
            emailid As String = Nothing,
            securitypin As String = Nothing
        Load_Cookie("LOAD", Clients_id_, clientsweb_id, Fidelity_id, Query_Langue,
                    emailid, securitypin, semp, SCompagnie)
        Start_Process(stream_api)
        Reset_Panel()
        Dim FUnite As String = Lblunite.InnerText
        Dim Lblunite_ As String = Lblunite.InnerText
        Dim ssortie As String = Lbldatesortie.InnerText
        Dim Lblclient_ As String = Lblclient.InnerText
        Dim FClient As String = Fiche_Lbl_Client_id.InnerText.Trim
        Dim BT_ As String = Request.QueryString("BT")
        Dim EMP As String = Request.QueryString("EMP")
        'semp = btn_EntreSortie_employe_id_1.InnerText
        BondeTravail = SortirJob_IDBondetravail.Value.Trim
        Load_Bon_Travail_Info(BondeTravail, LettreTravail)
#End Region

        clientsweb_id = GetColumnValueFromDB("clients", "Clients_id", Clients_id_, "Clients_noclient", "GSIGSI")
        semp = GetColumnValueFromDB("clientsweb", "clientsweb_id", clientsweb_id, "clientsweb_semp", "GSIGSI")
        SCompagnie = GetColumnValueFromDB("clientsweb", "clientsweb_id", clientsweb_id, "compagnieweb_index", "GSIGSI")

        Slienpath = GetColumnValueFromDB("compagnieweb", "compagnieweb_index", SCompagnie, "clientsweb_Slienpath", "GSIGSI")
        url_COB = GetColumnValueFromDB("compagnieweb", "compagnieweb_index", SCompagnie, "clientsweb_url", "GSIGSI")
        slienip = "http://" & url_COB & "/DevApi" & "/api/Person/"






        Try
            Select Case Scenario_
                Case "Page_Load"
                    Select Case True
                        Case Clients_id_ = Nothing
                            Load_Scenario_EXIT()
                            Exit Sub

                        Case Process_Count_Mysql("", "clientsweb_key", " WHERE clientsweb_id = '" & emailid & "' AND clientsweb_key = '" & Fidelity_id & "'") = 0
                            Load_Scenario_EXIT()
                            Exit Sub

                        Case SCompagnie = Nothing
                            clientsweb_id = clientsweb_id
                        Case Else
                            Dim Clients_email As String = emailid
                            Dim Count_ As Integer = Process_Count_Mysql("", "clientsweb_key", " WHERE compagnieweb_index = '" & SCompagnie & "' AND clientsweb_id = '" & Clients_email & "'")
                            Select Case True
                                Case Count_ = 1
                                Case Else



                                    Dim stg_ As String = Licence_Step_5(Scenario_, Clients_email, Fidelity_id, SCompagnie, clientsweb_id)
                                    Select Case True
                                        Case stg_ = "ERREUR" Or stg_ = "COMP-1000" Or stg_ = "EMP-1000"
                                            Fidelity_id = Fidelity_id
                                            Licence_Reaction(Fidelity_id)
                                            Creer_Usager_Alert.Visible = True
                                        Case Else
                                            Creer_Usager_Alert.Visible = False
                                    End Select

                            End Select


                    End Select
            End Select
        Catch ex As Exception

        End Try
        Dim ini_Scenario_ As String
        Try
            Dim Dict As New Dictionary(Of String, String) From {
    {"btn_Panel_Repeater_Metronic_allWO_Load_Jobs_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_TreeViewBtemploye_M_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_TreeViewBtemploye_M_reset_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_SortirdeJob_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"Filter_Letter_Tasks", "Load_Scenario_ONLIGNE_WORKING"},
    {"Repeaterrechercheproduit_ItemCommand", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_Entrerdespieces_Search_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_FicheProduits_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_FicheVehicules_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"SortirJob_IDBondetravail_Q2_Break_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_recherche_produit_back_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"SortirJob_IDBondetravail_Q2_Close_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"SortirJob_IDBondetravail_Q2_Continue_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_PunchInOut_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_Bontravail_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"Filter_Letter_Parts", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_Entrerdespieces_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_Entrerdestravails_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_details_emp_3_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_details_emp_2_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_Panel_PreFacture_ServerClick", "Load_Scenario_ONLIGNE_WORKING"},
    {"btn_BT_en_cours_employe_complet_ServerClick1", "Load_Scenario_ONLIGNE"},
    {"btn_wo_actuel_ServerClick", "Load_Scenario_ONLIGNE"},
    {"btn_Panel_Detail_ServerClick", "Load_Scenario_ONLIGNE"},
    {"btn_Panel_CreationBT_ServerClick", "Load_Scenario_ONLIGNE"},
    {"btn_Panel_CreationJOBS_ServerClick", "Load_Scenario_ONLIGNE"},
    {"btn_Panel_Entrerdestemps_ServerClick", "Load_Scenario_ONLIGNE"},
    {"Page_Load", "Page_Load"},
    {"btn_offline_panel_Agenda_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_offline_panel_Jobs_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_Panel_Montage_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_offline_panel_AllJobs_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_Panel_Montage_WEN_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_Panel_Menu_Taches_Offline_1_Menu_ServerClick", "Load_Scenario_OFFLINE"},
    {"btn_Panel_MenuWeb_Montage_Web_ServerClick", "Load_Scenario_OFFLINE"},
    {"KeyEND", "Value3"}
}
            ini_Scenario_ = GetValueIfKeyExists(Scenario_, Dict)
        Catch ex As Exception
        End Try
        Try
            Select Case ini_Scenario_
                Case "Key not found in dictionary."
                    Scenario_ = Scenario_
                    Load_Scenario(Scenario_)
                    Exit Sub
                Case "Page_Load"
                    Load_Scenario_PAGE_LOAD(Scenario_, Query_Langue, clientsweb_id, Clients_id_, Fidelity_id, semp, SCompagnie)
                Case "Load_Scenario_OFFLINE"
                    Load_Scenario_OFFLINE(Scenario_, Clients_id_, clientsweb_id, Fidelity_id, semp, SCompagnie)
                Case "Load_Scenario_ONLIGNE_WORKING"
                    Load_Scenario_ONLIGNE_WORKING(Scenario_, Clients_id_, BondeTravail, LettreTravail, FUnite, FClient, Fidelity_id, semp, ssortie, SCompagnie)
                Case "Load_Scenario_ONLIGNE"
                    Load_Scenario_ONLIGNE(Scenario_, Clients_id_, Fidelity_id, semp, ssortie, SCompagnie)
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btn_refresh_cookie_ServerClick(sender As Object, e As EventArgs)
        Load_UI_Cookie("WRITE", "Clients_web_step1", "")
        Load_UI_Cookie("WRITE", "Clients_web_step2", "")
        Load_UI_Cookie("WRITE", "Clients_web_step3", "")
        Load_UI_Cookie("WRITE", "Clients_web_step4", "")
        Load_UI_Cookie("WRITE", "Clients_web_step5", "")
        Load_UI_Cookie("WRITE", "compagnieweb_index", "")
    End Sub

    Protected Sub btn_Panel_FicheProduits_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_FicheProduits_ServerClick")
    End Sub

    Protected Sub btn_Panel_FicheVehicules_ServerClick(sender As Object, e As EventArgs)
        Load_Scenario("btn_Panel_FicheVehicules_ServerClick")

    End Sub

    Protected Sub Panel_Fiche_Produit_sauvegarder_ServerClick(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btn_Panel_MontageWeb_Fiche_back_Click(sender As Object, e As EventArgs)
        Panel_MontageWeb.Visible = True
        Panel_Fiche_Produit.Visible = False
    End Sub


End Class