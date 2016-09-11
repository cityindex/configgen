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
using System.Xml.Linq;
using ConfigGen.Domain.Contract.Settings;
using ConfigGen.Utilities.Annotations;

namespace ConfigGen.Templating.Xml.NodeProcessing
{
    public interface IConfigGenNodeProcessorFactory
    {
        /// <summary>
        /// Gets the processor for node.
        /// </summary>
        /// <param name="element">The node.</param>
        /// <returns>Processor for the supplied node</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="element"/> is not in the config-gen namespace.</exception>
        /// <exception cref="NotSupportedException">Thrown if no processor can be found for the supplied node.</exception>
        IConfigGenNodeProcessor GetProcessorForNode([NotNull] XElement element, [NotNull] IConfiguration configuration);
    }
}