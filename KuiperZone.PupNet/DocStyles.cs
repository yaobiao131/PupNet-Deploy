// -----------------------------------------------------------------------------
// SPDX-FileNotice: PupNet Deploy
// SPDX-License-Identifier: GPL-3.0-or-later
// SPDX-FileCopyrightText: © 2022-2026 Andrew Thomas <kuiperzone@users.noreply.github.com>
// SPDX-ProjectHomePage: https://github.com/kuiperzone/PupNet
// SPDX-FileType: Source
// SPDX-FileComment: This is NOT AI generated source code but was created with human thinking.
// -----------------------------------------------------------------------------

// PupNet is free software: you can redistribute it and/or modify it under
// the terms of the GNU Affero General Public License as published by the Free Software
// Foundation, either version 3 of the License, or (at your option) any later version.
//
// PupNet is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License along
// with PupNet. If not, see <https://www.gnu.org/licenses/>.

namespace KuiperZone.PupNet;

/// <summary>
/// Determines documentation string output.
/// </summary>
public enum DocStyles
{
    /// <summary>
    /// No documentation.
    /// </summary>
    NoComments = 0,

    /// <summary>
    /// Verbose output.
    /// </summary>
    Comments,

    /// <summary>
    /// Reference output.
    /// </summary>
    Reference,
}
