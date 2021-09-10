using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Web.UI;

namespace Utils
{
    public sealed class MessageBox
    {
        private static Hashtable m_executingPages = new Hashtable();
        private MessageBox() { }
        public static void Show(string message, string type = "", string title = "", string confirmButtonText = "", string function = "")
        {
            // If this is the first time a page has called this method then
            if (!m_executingPages.Contains(HttpContext.Current.Handler))
            {
                // Attempt to cast HttpHandler as a Page.
                Page executingPage = HttpContext.Current.Handler as Page;
                if (executingPage != null)
                {
                    string script = "swal({ title: '" + title + "', text: '" + message + "'";
                    if (type != "")
                        script += ", icon: '" + type + "'";
                    if (confirmButtonText != "")
                        script += ",  button: '" + confirmButtonText + "' })";
                    else
                        script += ",  button: 'Aceptar' })";
                    if (function != "")
                        script += ".then(function(value) {" + function + "});";
                    else
                        script += ";";
                    ScriptManager.RegisterStartupScript(executingPage, executingPage.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
            else
            {
                // If were here then the method has allready been
                // called from the executing Page.
                // We have allready created a message queue and stored a
                // reference to it in our hastable.
                Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
                // Add our message to the Queue
                queue.Enqueue(message);
            }
        }

        // Our page has finished rendering so lets output the
        // JavaScript to produce the alert's
        private static void ExecutingPage_Unload(object sender, EventArgs e)
        {
            // Get our message queue from the hashtable
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
            if (queue != null)
            {
                StringBuilder sb = new StringBuilder();
                // How many messages have been registered?
                int iMsgCount = queue.Count;
                // Use StringBuilder to build up our client slide JavaScript.
                sb.Append("<script language='javascript'>");
                // Loop round registered messages
                string sMsg;
                while (iMsgCount-- > 0)
                {
                    sMsg = (string)queue.Dequeue();
                    sMsg = sMsg.Replace("\n", "\\n");
                    sMsg = sMsg.Replace("\"", "'");
                    sb.Append(@"alert( """ + sMsg + @""" );");
                }
                // Close our JS
                sb.Append(@"</script>");
                // Were done, so remove our page reference from the hashtable
                m_executingPages.Remove(HttpContext.Current.Handler);
                // Write the JavaScript to the end of the response stream.
                HttpContext.Current.Response.Write(sb.ToString());
            }
        }

        public static void ShowInUpdatePanel(UpdatePanel panel, string message, string type = "", string title = "", string confirmButtonText = "", string function = "")
        {
            string script = "swal({ title: '" + title + "', text: '" + message + "'";
            if (type != "")
                script += ", icon: '" + type + "'";
            if (confirmButtonText != "")
                script += ",  button: '" + confirmButtonText + "' })";
            else
                script += ",  button: 'Aceptar' })";
            if (function != "")
                script += ".then(function(value)  {" + function + "});";
            else
                script += ";";
            ScriptManager.RegisterStartupScript(panel, panel.GetType(), Guid.NewGuid().ToString(), script, true);

        }

        public static void ShowConfirmation(string message, string title = "", string confirmButtonText = "", string cancelButtonText = "", string confirmFunction = "return false;", string cancelFunction = "return false;", bool withInputText = false, string inputTextPlaceHolder = "", string hiddenFieldId = "")
        {
            // If this is the first time a page has called this method then
            if (!m_executingPages.Contains(HttpContext.Current.Handler))
            {
                // Attempt to cast HttpHandler as a Page.
                Page executingPage = HttpContext.Current.Handler as Page;
                if (executingPage != null)
                {
                    string script = "";

                    if (withInputText && hiddenFieldId != "")
                    {
                        script += "var swalInput = document.createElement('input');";
                        script += "swalInput.setAttribute('placeholder', '";

                        if (inputTextPlaceHolder != "")
                            script += inputTextPlaceHolder;
                        else
                            script += "Ingrese el número de confirmación";

                        script += "');";
                        script += "swalInput.setAttribute('class', 'swal-content__input');";
                    }

                    script += "swal({ title: '" + title + "', text: '" + message + "'";
                    script += ", icon: 'warning'";
                    script += ", buttons: { confirm: { text:'";

                    if (confirmButtonText != "")
                        script += confirmButtonText + "'";
                    else
                        script += "Aceptar'";

                    script += ", value: 'confirm', visible: true}, cancel: { text: '";

                    if (cancelButtonText != "")
                        script += cancelButtonText + "'";
                    else
                        script += "Cancelar'";

                    script += " , value: 'cancel', visible: true}}";

                    if (withInputText && hiddenFieldId != "")
                        script += ", content: swalInput";

                    // script += "}).then((value) => { switch(value) { case 'confirm':";                 
                    script += "}).then(function(value) { switch(value) { case 'confirm':";

                    if (confirmFunction != "")
                    {
                        if (withInputText && hiddenFieldId != "")
                        {
                            script += "$('#" + hiddenFieldId + "').val(swalInput.value);";
                        }
                        script += confirmFunction;
                    }
                    else
                        script += "return false;";

                    script += " break; case 'cancel':";

                    if (cancelFunction != "")
                        script += cancelFunction;
                    else
                        script += "return false;";

                    script += " break; default: return false; } });";

                    ScriptManager.RegisterStartupScript(executingPage, executingPage.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
            else
            {
                // If were here then the method has allready been
                // called from the executing Page.
                // We have allready created a message queue and stored a
                // reference to it in our hastable.
                Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
                // Add our message to the Queue
                queue.Enqueue(message);
            }
        }

        public static void ShowConfirmationInUpdatePanel(UpdatePanel panel, string message, string title = "", string confirmButtonText = "", string cancelButtonText = "", string confirmFunction = "return false;", string cancelFunction = "return false;", bool withInputText = false, string inputTextPlaceHolder = "", string hiddenFieldId = "")
        {
            string script = "";

            if (withInputText && hiddenFieldId != "")
            {
                script += "var swalInput = document.createElement('input');";
                script += "swalInput.setAttribute('placeholder', '";

                if (inputTextPlaceHolder != "")
                    script += inputTextPlaceHolder;
                else
                    script += "Ingrese el número de confirmación";

                script += "');";
                script += "swalInput.setAttribute('class', 'swal-content__input');";
            }

            script += "swal({ title: '" + title + "', text: '" + message + "'";
            script += ", icon: 'warning'";
            script += ", buttons: { confirm: { text:'";

            if (confirmButtonText != "")
                script += confirmButtonText + "'";
            else
                script += "Aceptar'";

            script += ", value: 'confirm', visible: true}, cancel: { text: '";

            if (cancelButtonText != "")
                script += cancelButtonText + "'";
            else
                script += "Cancelar'";

            script += " , value: 'cancel', visible: true}}";

            if (withInputText && hiddenFieldId != "")
                script += ", content: swalInput";

            script += "}).then(function(value) { switch(value) { case 'confirm':";

            if (confirmFunction != "")
            {
                if (withInputText && hiddenFieldId != "")
                {
                    script += "$('#" + hiddenFieldId + "').val(swalInput.value);";
                }
                script += confirmFunction;
            }
            else
                script += "return false;";

            script += " break; case 'cancel':";

            if (cancelFunction != "")
                script += cancelFunction;
            else
                script += "return false;";

            script += " break; default: return false; } });";

            ScriptManager.RegisterStartupScript(panel, panel.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}