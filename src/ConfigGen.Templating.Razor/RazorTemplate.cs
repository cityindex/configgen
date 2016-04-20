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

using System;
using System.Collections.Generic;
using System.Linq;
using ConfigGen.Domain.Contract;
using ConfigGen.Infrastructure.RazorTemplateRendering;
using JetBrains.Annotations;

namespace ConfigGen.Templating.Razor
{
    public class RazorTemplate : ITemplate
    {
        [NotNull]
        private const string RazorTemplateErrorSource = nameof(RazorTemplate);

        [Pure]
        [NotNull]
        public RenderResults Render([NotNull] string templateContents, [NotNull] [ItemNotNull] IEnumerable<IConfiguration> configurationsToRender)
        {
            if (configurationsToRender == null) throw new ArgumentNullException(nameof(configurationsToRender));

            var razorTemplateRenderer = new RazorTemplateRenderer(templateContents);

            var allResults = configurationsToRender.Select(configuration => RenderSingleConfiguration(razorTemplateRenderer, configuration)).ToList();

            return new RenderResults(TemplateRenderResultStatus.Success, allResults, null);
        }

        private SingleTemplateRenderResults RenderSingleConfiguration([NotNull] RazorTemplateRenderer razorTemplateRenderer, [NotNull] IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (razorTemplateRenderer == null) throw new ArgumentNullException(nameof(razorTemplateRenderer));

            try
            {
                var settings = configuration.ToDictionary();
                var model = new DictionaryBackedDynamicModel(settings);
                RenderingResult result = razorTemplateRenderer.Render(model);

                var usedTokens = new List<string>();
                var unusedTokens = new List<string>();

                foreach (var settingName in settings)
                {
                    if (model.AccessedTokens.Contains(settingName.Key))
                    {
                        usedTokens.Add(settingName.Key);
                    }
                    else
                    {
                        unusedTokens.Add(settingName.Key);
                    }
                }

                if (result.Status == RenderingResultStatus.Success)
                {
                    return new SingleTemplateRenderResults(
                        status: TemplateRenderResultStatus.Success,
                        renderedResult: result.RenderedResult,
                        usedTokens: usedTokens,
                        unusedTokens: unusedTokens,
                        unrecognisedTokens: model.UnrecognisedTokens,
                        errors: null);
                }

                return new SingleTemplateRenderResults(
                    status: TemplateRenderResultStatus.Failure,
                    renderedResult: null,
                    usedTokens: usedTokens,
                    unusedTokens: unusedTokens,
                    unrecognisedTokens: model.UnrecognisedTokens,
                    errors: MapErrors(result));
            }
            catch (Exception ex)
            {
                return new SingleTemplateRenderResults(
                    status: TemplateRenderResultStatus.Failure,
                    renderedResult: null,
                    usedTokens: null,
                    unusedTokens: null,
                    unrecognisedTokens: null,
                    errors: new[] { new UnhandledExceptionError(RazorTemplateErrorSource, ex) });
            }
        }

        [NotNull]
        private IEnumerable<Error> MapErrors([NotNull] RenderingResult renderingResult)
        {
            if (renderingResult == null) throw new ArgumentNullException(nameof(renderingResult));

            var detail = string.Join("\n", renderingResult.Errors ?? new string[0]);
            if (renderingResult.Status == RenderingResultStatus.CodeCompilationFailed)
            {
                yield return new RazorTemplateError(RazorTemplateErrorCodes.CodeCompilationError, detail);
            }
            else if (renderingResult.Status == RenderingResultStatus.CodeGenerationFailed)
            {
                yield return new RazorTemplateError(RazorTemplateErrorCodes.CodeGenerationError, detail);
            }
            else
            {
                yield return new RazorTemplateError(RazorTemplateErrorCodes.GeneralRazorTemplateError, detail);
            }
        }
    }
}