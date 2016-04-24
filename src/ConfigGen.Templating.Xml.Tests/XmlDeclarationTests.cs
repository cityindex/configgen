﻿#region Copyright and License Notice
// Copyright (C)2010-2016 - INEX Solutions Ltd
// https://github.com/inex-solutions/configgen
// 
// This file is part of ConfigGen.
// 
// ConfigGen is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ConfigGen is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License and 
// the GNU Lesser General Public License along with ConfigGen.  
// If not, see <http://www.gnu.org/licenses/>
#endregion

using System.Collections.Generic;
using ConfigGen.Domain.Contract;
using ConfigGen.Tests.Common.MSpec;
using Machine.Specifications;
using Machine.Specifications.Model;

namespace ConfigGen.Templating.Xml.Tests
{
    namespace XmlDeclarationTests
    {
        public class when_the_template_contains_an_xml_declaration : XmlTemplateTestsBase
        {
            private static string XmlDeclaration;
            private static string TemplateBody;
            Establish context = () =>
            {
                XmlDeclaration = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                TemplateBody = 
@"<root>
  <child key=""value"" />
</root>";
                TemplateContents = XmlDeclaration + TemplateBody;
                SingleConfiguration = new Dictionary<string, object>();
            };

            Because of = () => Results = Subject.Render(TemplateContentsAsStream, Configurations);

            It there_should_be_a_single_render_result = () => Results.Count.ShouldEqual(1);

            It the_render_should_be_successful = () => FirstResult.Status.ShouldEqual(TemplateRenderResultStatus.Success);

            It the_resulting_output_should_contain_the_xml_declaration = 
                () => FirstResult.RenderedResult.ShouldStartWith(XmlDeclaration);

            It the_resulting_output_should_be_the_rest_of_the_template_unaltered =
                () => FirstResult.RenderedResult.ShouldContainXml(TemplateContents);
        }
    }
}
