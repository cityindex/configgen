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
using JetBrains.Annotations;

namespace ConfigGen.Domain.Contract
{
    public abstract class Error
    {
        protected Error([NotNull] string source, [NotNull] string code, [CanBeNull] string detail, [CanBeNull] IEnumerable<Error> subErrors = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (detail == null) throw new ArgumentNullException(nameof(detail));

            Source = source;
            Code = code;
            Detail = detail;
            SubErrors = subErrors ?? Enumerable.Empty<Error>();
        }


        [NotNull]
        public string Source { get; }

        [NotNull]
        public string Code { get; }

        [CanBeNull]
        public string Detail { get; }

        [NotNull]
        public IEnumerable<Error> SubErrors { get; }

        public override string ToString()
        {
            var msg = $"Error '{Code}' in '{Source}': {Detail}";

            return SubErrors.Aggregate(msg, (current, subError) => current + $"\n --Sub Error --\n{subError}");
        }
    }
}
