#pragma checksum "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6757447183aa9b6f934a73ea2f5bf15b6bbf7e6e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cars_EditCar), @"mvc.1.0.view", @"/Views/Cars/EditCar.cshtml")]
namespace AspNetCore
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
#line 1 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\_ViewImports.cshtml"
using CarShop.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\_ViewImports.cshtml"
using CarShop.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
using CarShop.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6757447183aa9b6f934a73ea2f5bf15b6bbf7e6e", @"/Views/Cars/EditCar.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"20b81c75a5b07aef8b59bfd373079ebaf05b028a", @"/Views/_ViewImports.cshtml")]
    public class Views_Cars_EditCar : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Car>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditCar", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Cars", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
  
    ViewData["Title"] = "Edit car page";

#line default
#line hidden
#nullable disable
            WriteLiteral("<h1>Edit car page</h1>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6757447183aa9b6f934a73ea2f5bf15b6bbf7e6e4315", async() => {
                WriteLiteral("\r\n    <div class=\"form-group\">\r\n        <input id=\"carId\" name=\"carId\"");
                BeginWriteAttribute("value", " value=\"", 227, "\"", 244, 1);
#nullable restore
#line 9 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 235, Model.Id, 235, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"hidden\" />\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <label for=\"carMake\">Car make</label>\r\n        <input id=\"carMake\" name=\"carMake\"");
                BeginWriteAttribute("value", " value=\"", 395, "\"", 414, 1);
#nullable restore
#line 13 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 403, Model.Make, 403, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"Text\" />\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <label for=\"carModel\">Car model</label>\r\n        <input id=\"carModel\" name=\"carModel\"");
                BeginWriteAttribute("value", " value=\"", 567, "\"", 587, 1);
#nullable restore
#line 17 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 575, Model.Model, 575, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"Text\" />\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <label for=\"carYear\">Car year</label>\r\n        <input id=\"carYear\" name=\"carYear\"");
                BeginWriteAttribute("value", " value=\"", 736, "\"", 755, 1);
#nullable restore
#line 21 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 744, Model.Year, 744, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"Text\" />\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <label for=\"carPictureURL\">Car picture URL</label>\r\n        <input id=\"carPictureURL\" name=\"carPictureURL\"");
                BeginWriteAttribute("value", " value=\"", 929, "\"", 954, 1);
#nullable restore
#line 25 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 937, Model.PictureURL, 937, 17, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"Text\" />\r\n    </div>\r\n    <div class=\"form-group\">\r\n        <label for=\"carOwner\">Owner</label>\r\n        <input id=\"carOwner\" name=\"carOwner\"");
                BeginWriteAttribute("value", " value=\"", 1103, "\"", 1123, 1);
#nullable restore
#line 29 "D:\SoftUni\DataBaseFundamentals-C#\BasicWebApp\CarShopApp\CarShop.Web\Views\Cars\EditCar.cshtml"
WriteAttributeValue("", 1111, Model.Owner, 1111, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" type=\"Text\" />\r\n    </div>\r\n    <input value=\"Save\" type=\"Submit\" class=\"btn btn-primary\" />\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("   ");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Car> Html { get; private set; }
    }
}
#pragma warning restore 1591
