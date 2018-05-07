﻿#region Copyright and Licence Notice
// Copyright (C)2010-2018 - INEX Solutions Ltd
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

using System.Threading.Tasks;
using ConfigGen.Application.Test.Common;
using ConfigGen.Application.Test.Common.Specification;
using ConfigGen.Utilities;
using Shouldly;

namespace ConfigGen.Application.Test.SimpleTests
{
    public class given_a_spreadsheet_with_two_entries : ApplicationTestBase
    {
        private string testFileContents;

        protected override async Task Given()
        {
            testFileContents = @"Test File Contents";

            await TemplateFileContains(testFileContents);

            await SettingsFileContains(@"
Filename    | Col1   | Col2
            |        |
App1.Config | Val1-1 | Val1-2
App2.Config | Val2-1 | Val2-2");

            SetOutputDirectory(TestDirectory.FullName);
            SetSettingsFilePath(TestDirectory.File("App.Config.Settings.xlsx"));
            SetTemplateFilePath(TestDirectory.File("App.Config.Template.razor"));
        }

        protected override async Task When() => Result = await ConfigGenService.GenerateConfigurations(Options);

        [Then]
        public void the_result_reports_two_files_were_generated_with_the_names_specified() => Result.ShouldHaveGenerated(2).Files.Named("App1.Config","App2.Config");

        [Then]
        public void the_first_generated_config_file_contains_the_template_contents() => TestDirectory.File("App1.Config").ShouldHaveContents(testFileContents);

        [Then]
        public void the_second_generated_config_file_contains_the_template_contents() => TestDirectory.File("App2.Config").ShouldHaveContents(testFileContents);
    }
}
 