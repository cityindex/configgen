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
using System.Collections.Immutable;
using System.Linq;
using ConfigGen.Application.Contract;
using ConfigGen.Domain.Contract;
using ConfigGen.Utilities.EventLogging;

namespace ConfigGen.Application
{
    public class GenerationResultFactory
    {
        private readonly IReadableEventLogger _eventLogger;

        public GenerationResultFactory(IReadableEventLogger eventLogger)
        {
            _eventLogger = eventLogger;
        }

        public SingleConfigurationGenerationResult CreateResult(RenderResult renderResult)
        {
            var events = _eventLogger.LoggedEvents
                .OfType<IConfigurationSpecificEvent>()
                .Where(e => e.ConfigurationIndex == renderResult.Configuration.Index)
                .Distinct()
                .ToList();

            var alltokens = renderResult.Configuration.Settings.Keys.ToImmutableHashSet();
            var usedTokens = events.OfType<TokenUsedEvent>().Select(t => t.TokenName).ToImmutableHashSet();
            var unrecognisedTokens = events.OfType<UnrecognisedTokenEvent>().Select(t => t.TokenName).ToImmutableHashSet();
            var unusedTokens = alltokens.Except(usedTokens);

            return new SingleConfigurationGenerationResult(
                renderResult.Configuration.Index,
                renderResult.Configuration.ConfigurationName,
                renderResult.WriteResult.FileName,
                renderResult.Configuration.Settings,
                usedTokens,
                unusedTokens,
                unrecognisedTokens);
        }
    }
}