#pragma checksum "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ecf7c436871149e07fe1d032d5ca0da0eb0a171f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(FactuLib.Areas.Users.Pages.Account.Account.Areas_Users_Pages_Account_detailsUser), @"mvc.1.0.razor-page", @"/Areas/Users/Pages/Account/detailsUser.cshtml")]
namespace FactuLib.Areas.Users.Pages.Account.Account
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\_ViewImports.cshtml"
using FactuLib.Areas.Users.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ecf7c436871149e07fe1d032d5ca0da0eb0a171f", @"/Areas/Users/Pages/Account/detailsUser.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a292a4b27a9526d6f339774019eef19bac90c25a", @"/Areas/Users/Pages/_ViewImports.cshtml")]
    public class Areas_Users_Pages_Account_detailsUser : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "Users", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-id", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "Register", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-id", "2", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
  
    var image = "data:image/jpg;base64," + Convert.ToBase64String(Model.Input.DataUser.Image, 0, Model.Input.DataUser.Image.Length);
    var name = Model.Input.DataUser.Name + " " + Model.Input.DataUser.LastName;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container\">\r\n    <h1>");
#nullable restore
#line 9 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
   Write(name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <div class=\"row\">\r\n        <div class=\"col-sm\">\r\n            <div class=\"card text-center\" style=\"width: 21rem;\">\r\n                <div class=\"card-header\">\r\n                    <img class=\"imageUserDetail\"");
            BeginWriteAttribute("src", " src=\"", 511, "\"", 523, 1);
#nullable restore
#line 14 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
WriteAttributeValue("", 517, image, 517, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
                </div>
                <div class=""card text-center"">
                    <table class=""tableCursos"">
                        <tbody>
                            <tr>
                                <td>
                                    <p>");
#nullable restore
#line 21 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(Model.Input.DataUser.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                </td>\r\n                            </tr>\r\n                            <tr>\r\n                                <td>\r\n                                    <p>");
#nullable restore
#line 26 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(Model.Input.DataUser.Role);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                </td>\r\n                            </tr>\r\n                            <tr>\r\n                                <td>\r\n");
#nullable restore
#line 31 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                     if (Model.Input.DataUser.IdentityUser.LockoutEnabled)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <p class=\"text-success\">Disponible</p>\r\n");
#nullable restore
#line 34 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                    }
                                    else
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <p class=\"text-danger\">No Disponible</p>\r\n");
#nullable restore
#line 38 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class=""col-sm"">
            <div class=""card"">
                <div class=""card-body"">
                    <table class=""tableCursos"">
                        <tbody>
                            <tr>
                                <th>
                                    Informacion
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    NID
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <p>");
#nullable restore
#line 63 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(Model.Input.DataUser.NID);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Nombre
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <p>");
#nullable restore
#line 73 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(name);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Telefono
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <p>");
#nullable restore
#line 83 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(Model.Input.DataUser.IdentityUser.PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Correo Electronico
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <p>");
#nullable restore
#line 93 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                  Write(Model.Input.DataUser.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                </td>\r\n                            </tr>\r\n                            <tr>\r\n                                <td>\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecf7c436871149e07fe1d032d5ca0da0eb0a171f11922", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 99 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                          
                                            var dataUser = JsonConvert.SerializeObject(Model.Input.DataUser);
                                        

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <input type=\"hidden\" name=\"DataUser\"");
                BeginWriteAttribute("value", " value=\"", 4315, "\"", 4332, 1);
#nullable restore
#line 102 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
WriteAttributeValue("", 4323, dataUser, 4323, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
                                        <button type=""submit"" value=""Editar"" class=""btn btn-success btn-sm"">
                                            <i class=""bi bi-pencil""></i> Editar
                                        </button>
                                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Page = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecf7c436871149e07fe1d032d5ca0da0eb0a171f15532", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 110 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
                                          
                                            var dataUser2 = JsonConvert.SerializeObject(Model.Input.DataUser);
                                        

#line default
#line hidden
#nullable disable
                WriteLiteral("                                        <input type=\"hidden\" name=\"DataUser\"");
                BeginWriteAttribute("value", " value=\"", 5089, "\"", 5107, 1);
#nullable restore
#line 113 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
WriteAttributeValue("", 5097, dataUser2, 5097, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                                        <button type=\"submit\" value=\"Borrar\" class=\"btn btn-danger btn-sm\"");
                BeginWriteAttribute("onclick", " onclick=\"", 5219, "\"", 5306, 8);
                WriteAttributeValue("", 5229, "return", 5229, 6, true);
                WriteAttributeValue(" ", 5235, "confirm(\'Seguro", 5236, 16, true);
                WriteAttributeValue(" ", 5251, "de", 5252, 3, true);
                WriteAttributeValue(" ", 5254, "eliminar", 5255, 9, true);
                WriteAttributeValue(" ", 5263, "el", 5264, 3, true);
                WriteAttributeValue(" ", 5266, "usuario:", 5267, 9, true);
#nullable restore
#line 114 "C:\Users\pafla\source\repos\FactuLib\FactuLib\Areas\Users\Pages\Account\detailsUser.cshtml"
WriteAttributeValue(" ", 5275, Model.Input.DataUser.Email, 5276, 27, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue(" ", 5303, "\')", 5304, 3, true);
                EndWriteAttribute();
                WriteLiteral(">\r\n                                            <i class=\"bi bi-trash3-fill\"></i> Borrar\r\n                                        </button>\r\n                                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Page = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<detailsUserModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<detailsUserModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<detailsUserModel>)PageContext?.ViewData;
        public detailsUserModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
