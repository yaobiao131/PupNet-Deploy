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

namespace KuiperZone.PupNet.Builders;

/// <summary>
/// Extends <see cref="PackageBuilder"/> for Zip package.
/// </summary>
public sealed class ZipBuilder : PackageBuilder
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ZipBuilder(ConfigurationReader conf)
        : base(conf, conf.Arguments.Kind)
    {
        if (Kind != PackageKind.Zip && Kind != PackageKind.Gz)
        {
            throw new ArgumentException($"Invalid package kind {Kind}", nameof(conf));
        }

        BuildAppBin = Path.Combine(BuildRoot, "Publish");

        // Not used
        InstallBin = "";

        // Not used
        ManifestBuildPath = null;
        ManifestContent = null;
        PackageCommands = Array.Empty<string>();
    }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string PackageArch
    {
        get
        {
            if (Arguments.Arch != null)
            {
                return Arguments.Arch;
            }

            return Runtime.BuildArch.ToString().ToLowerInvariant();
        }
    }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string OutputName
    {
        get
        {
            var ext = Kind == PackageKind.Zip ? ".zip" : ".tar.gz";
            return GetOutputName(Configuration.ZipVersionOutput, Runtime.RuntimeId, ext);
        }
    }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string BuildAppBin { get; }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string InstallBin { get; }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string? ManifestBuildPath { get; }

    /// <summary>
    /// Implements.
    /// </summary>
    public override string? ManifestContent { get; }

    /// <summary>
    /// Implements.
    /// </summary>
    public override IReadOnlyCollection<string> PackageCommands { get; }

    /// <summary>
    /// Implements.
    /// </summary>
    public override bool SupportsStartCommand { get; } = false;

    /// <summary>
    /// Implements.
    /// </summary>
    public override bool SupportsPostRun { get; } = true;

    /// <summary>
    /// Overrides and extends.
    /// </summary>
    public override void BuildPackage()
    {
        BuildPackage(false);

        if (Kind == PackageKind.Gz)
        {
            Operations.TarGz(BuildAppBin, OutputPath);
        }
        else
        {
            Operations.Zip(BuildAppBin, OutputPath);
        }

        BuildArtifacts();

        if (Arguments.IsRun)
        {
            // Just run the build
            Directory.SetCurrentDirectory(BuildAppBin);
            Operations.Execute(AppExecName);
        }
    }

}
