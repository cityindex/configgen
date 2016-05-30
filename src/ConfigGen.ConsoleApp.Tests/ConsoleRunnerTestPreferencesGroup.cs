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

namespace ConfigGen.ConsoleApp.Tests
{
    public class ConsoleRunnerTestPreferencesGroup : PreferenceGroupBase
    {
        private static string PreferenceGroupName = "ConsoleRunnerTestPreferencesGroup";

        public ConsoleRunnerTestPreferencesGroup()
        {

            StringParameterPreference = new PreferenceDefinition<ConsoleRunnerTestPreferences, string>(
                preferenceGroupName: PreferenceGroupName,
                name: "StringParameter",
                shortName: "String",
                description: "specifies the string parameter",
                parameters: new[,] {{"<parameter value>", "the value of the parameter"}},
                parseAction: argsQueue => argsQueue.ParseSingleStringParameterFromArgumentQueue("StringParameter"),
                setAction: (preferences, value) => preferences.StringParameter = value);

            BooleanSwitchPreference = new PreferenceDefinition<ConsoleRunnerTestPreferences, bool>(
                preferenceGroupName: PreferenceGroupName,
                name: "BooleanSwitch",
                shortName: "Boolean",
                description: "a switch",
                parameters: new[,] {{"[true|false]", "[optional] the value for the switch, default true."}},
                parseAction: argsQueue => argsQueue.ParseSwitchParameterFromArgumentQueue("BooleanSwitch"),
                setAction: (preferences, value) => preferences.BooleanSwitch = value);
        }

        public PreferenceDefinition<ConsoleRunnerTestPreferences, bool> BooleanSwitchPreference { get; }

        public PreferenceDefinition<ConsoleRunnerTestPreferences, string> StringParameterPreference { get; }

        public override string Name => "ConsoleRunnerTestPreferencesGroup";

        protected override IEnumerable<IPreferenceDefinition> Preferences => new IPreferenceDefinition[] {StringParameterPreference, BooleanSwitchPreference};
    }
}