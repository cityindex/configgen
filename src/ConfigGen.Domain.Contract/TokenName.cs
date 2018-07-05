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

using System;

namespace ConfigGen.Domain.Contract
{
    public sealed class TokenName : IEquatable<TokenName>, IComparable<TokenName>
    {
        private readonly string _tokenName;

        public TokenName(string tokenName)
        {
            _tokenName = tokenName;
        }

        public bool Equals(TokenName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_tokenName, other._tokenName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TokenName) obj);
        }

        public override int GetHashCode()
        {
            return (_tokenName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(_tokenName) : 0);
        }

        public int CompareTo(TokenName other)
        {
            return string.Compare(_tokenName, other._tokenName, StringComparison.OrdinalIgnoreCase);
        }

        public static implicit operator string(TokenName tokenName)
        {
            return tokenName._tokenName;
        }

        public static explicit operator TokenName(string tokenName)
        {
            return new TokenName(tokenName);
        }

        public override string ToString()
        {
            return _tokenName;
        }
    }
}