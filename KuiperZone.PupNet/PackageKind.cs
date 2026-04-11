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

using System.Runtime.InteropServices;

namespace KuiperZone.PupNet;

/// <summary>
/// Defines deployable package kinds.
/// </summary>
public enum PackageKind
{
    /// <summary>
    /// Windows zip file on win builds, tar.gz on all others.
    /// </summary>
    Auto = 0,

    /// <summary>
    /// Zip file on all platforms.
    /// </summary>
    Zip,

    /// <summary>
    /// Tar.gz on all platforms.
    /// </summary>
    Gz,

    /// <summary>
    /// AppImage. Linux only.
    /// </summary>
    AppImage,

    /// <summary>
    /// Debian package. Linux only.
    /// </summary>
    Deb,

    /// <summary>
    /// RPM package. Linux only.
    /// </summary>
    Rpm,

    /// <summary>
    /// Flatpak. Linux only.
    /// </summary>
    Flatpak,

    /// <summary>
    /// Setup file. Windows only.
    /// </summary>
    Setup,
}

/// <summary>
/// Extension methods.
/// </summary>
public static class DeployKindExtension
{
    /// <summary>
    /// Gets whether compatible with linux.
    /// </summary>
    public static bool CanTargetLinux(this PackageKind kind)
    {
        switch (kind)
        {
            case PackageKind.Zip:
            case PackageKind.Gz:
            case PackageKind.AppImage:
            case PackageKind.Deb:
            case PackageKind.Rpm:
            case PackageKind.Flatpak:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Gets whether exclusive to linux.
    /// </summary>
    public static bool IsLinuxExclusive(this PackageKind kind)
    {
        return kind != PackageKind.Zip && CanTargetLinux(kind);
    }

    /// <summary>
    /// Gets whether compatible with Window.
    /// </summary>
    public static bool CanTargetWindows(this PackageKind kind)
    {
        return kind == PackageKind.Zip || kind == PackageKind.Setup;
    }

    /// <summary>
    /// Gets whether exclusive to Windows.
    /// </summary>
    public static bool IsWindowsExclusive(this PackageKind kind)
    {
        return kind != PackageKind.Zip && CanTargetWindows(kind);
    }

    /// <summary>
    /// Gets whether compatible with OSX.
    /// </summary>
    public static bool CanTargetOsx(this PackageKind kind)
    {
        return kind == PackageKind.Zip;
    }

    /// <summary>
    /// Gets whether exclusive to OSX.
    /// </summary>
    public static bool IsOsxExclusive(this PackageKind _)
    {
        return false;
    }

    /// <summary>
    /// Returns true if the package kind can be built on this system.
    /// </summary>
    public static bool CanBuildOnSystem(this PackageKind kind)
    {
        if (kind.CanTargetLinux() && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return true;
        }

        if (kind.CanTargetWindows() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return true;
        }

        if (kind.CanTargetOsx() && RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the package kind can be GPG signed.
    /// </summary>
    public static bool CanGpgSign(this PackageKind kind)
    {
        // Include future BSD?
        return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            (kind == PackageKind.AppImage || kind == PackageKind.Rpm || kind == PackageKind.Flatpak);
    }

}