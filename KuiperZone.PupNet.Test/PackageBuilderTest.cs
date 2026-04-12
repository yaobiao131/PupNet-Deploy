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
using KuiperZone.PupNet.Builders;
using Xunit;

namespace KuiperZone.PupNet.Test;

public class PackageBuilderTest
{
    [Fact]
    public void DefaultIcons_Available()
    {
        Assert.NotEmpty(PackageBuilder.DefaultTerminalIcons);

        foreach (var item in PackageBuilder.DefaultTerminalIcons)
        {
            Assert.True(File.Exists(item));
        }

        Assert.NotEmpty(PackageBuilder.DefaultGuiIcons);

        foreach (var item in PackageBuilder.DefaultGuiIcons)
        {
            Assert.True(File.Exists(item));
        }
    }

    [Fact]
    public void AppImage_DecodesOK()
    {
        var builder = new AppImageBuilder(new DummyConf());

        AssertOK(builder, PackageKind.AppImage);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.EndsWith("usr/share/metainfo/com.example.helloworld.appdata.xml", builder.MetaBuildPath);
        }

        // Skip arch - depends on test system -- covered in other tests
        Assert.StartsWith("HelloWorld-5.4.3-2.", builder.OutputName);
        Assert.EndsWith(".AppImage", builder.OutputName);
    }

    [Fact]
    public void Flatpak_DecodesOK()
    {
        var builder = new FlatpakBuilder(new DummyConf());

        AssertOK(builder, PackageKind.Flatpak);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.EndsWith("usr/share/metainfo/com.example.helloworld.metainfo.xml", builder.MetaBuildPath);
            Assert.EndsWith("com.example.helloworld-linux-x64-Release-Flatpak/com.example.helloworld.yml", builder.ManifestBuildPath);
        }

        Assert.StartsWith("HelloWorld-5.4.3-2.", builder.OutputName);
        Assert.EndsWith(".flatpak", builder.OutputName);
    }

    [Fact]
    public void Rpm_DecodesOK()
    {
        var builder = new RpmBuilder(new DummyConf());

        AssertOK(builder, PackageKind.Rpm);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.EndsWith("usr/share/metainfo/com.example.helloworld.metainfo.xml", builder.MetaBuildPath);
            Assert.EndsWith("com.example.helloworld-linux-x64-Release-Rpm/com.example.helloworld.spec", builder.ManifestBuildPath);
        }

        Assert.StartsWith("helloworld_5.4.3-2", builder.OutputName);
        Assert.EndsWith(".rpm", builder.OutputName);
    }

    [Fact]
    public void Debian_DecodesOK()
    {
        var builder = new DebianBuilder(new DummyConf());

        AssertOK(builder, PackageKind.Deb);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.EndsWith("usr/share/metainfo/com.example.helloworld.metainfo.xml", builder.MetaBuildPath);
            Assert.EndsWith("nux-x64-Release-Deb/AppDir/DEBIAN/control", builder.ManifestBuildPath);
        }

        Assert.StartsWith("helloworld_5.4.3-2", builder.OutputName);
        Assert.EndsWith(".deb", builder.OutputName);
    }

    [Fact]
    public void Setup_DecodesOK()
    {
        var builder = new SetupBuilder(new DummyConf());

        AssertOK(builder, PackageKind.Setup);
        Assert.Null(builder.MetaBuildPath);

        Assert.StartsWith("HelloWorldSetup-5.4.3-2.", builder.OutputName);
        Assert.EndsWith(".exe", builder.OutputName);
    }

    [Fact]
    public void Zip_DecodesOK()
    {
        var builder = new ZipBuilder(new DummyConf(PackageKind.Zip, nameof(ConfigurationReader.ZipVersionOutput)));
        AssertOK(builder, PackageKind.Zip);
        Assert.Null(builder.MetaBuildPath);

        Assert.StartsWith("HelloWorld-5.4.3-2.", builder.OutputName);
        Assert.EndsWith(".zip", builder.OutputName);
    }

    [Fact]
    public void Gz_DecodesOK()
    {
        var builder = new ZipBuilder(new DummyConf(PackageKind.Gz, nameof(ConfigurationReader.ZipVersionOutput)));
        AssertOK(builder, PackageKind.Gz);
        Assert.Null(builder.MetaBuildPath);

        Assert.StartsWith("HelloWorld-5.4.3-2.", builder.OutputName);
        Assert.EndsWith(".tar.gz", builder.OutputName);
    }

    private static void AssertOK(PackageBuilder builder, PackageKind kind)
    {
        Assert.Equal(kind, builder.Kind);
        Assert.Equal(kind.CanTargetWindows(), !builder.Kind.IsLinuxExclusive());

        var appExecName = builder.Runtime.IsWindowsRuntime ? "HelloWorld.exe" : "HelloWorld";
        Assert.Equal(appExecName, builder.AppExecName);
        Assert.Equal("5.4.3", builder.AppVersion);
        Assert.Equal("2", builder.PackageRelease);

        // Not fully qualified as no assert files
        Assert.Equal("Deploy", builder.OutputDirectory);

        if (builder.Kind.IsLinuxExclusive() && kind != PackageKind.Gz)
        {
            Assert.EndsWith($"usr/bin", builder.BuildUsrBin);
            Assert.EndsWith($"usr/share", builder.BuildUsrShare);
            Assert.EndsWith($"usr/share/metainfo", builder.BuildShareMeta);
            Assert.EndsWith($"usr/share/applications", builder.BuildShareApplications);
            Assert.EndsWith($"usr/share/icons", builder.BuildShareIcons);

            Assert.EndsWith($"usr/share/applications/com.example.helloworld.desktop", builder.DesktopBuildPath);


            Assert.Equal($"Assets/Icon.svg", builder.PrimaryIcon);

            Assert.Contains($"Assets/Icon.svg", builder.IconPaths.Keys);
            Assert.Contains($"Assets/Icon.32x32.png", builder.IconPaths.Keys);
            Assert.Contains($"Assets/Icon.x48.png", builder.IconPaths.Keys);
            Assert.Contains($"Assets/Icon.64.png", builder.IconPaths.Keys);

            // Excluded on windows
            Assert.DoesNotContain($"Assets/Icon.ico", builder.IconPaths.Keys);
        }
        else
        if (builder.Kind.IsWindowsExclusive())
        {
            Assert.Null(builder.BuildUsrBin);
            Assert.Null(builder.BuildUsrShare);
            Assert.Null(builder.BuildShareMeta);
            Assert.Null(builder.BuildShareApplications);
            Assert.Null(builder.BuildShareIcons);

            Assert.Null(builder.DesktopBuildPath);

            Assert.Equal($"Assets/Icon.ico", builder.PrimaryIcon);

            // These are empty on non-linus, only has PrimaryIcon
            Assert.Empty(builder.IconPaths);
        }
        else
        if (builder.Kind.IsOsxExclusive())
        {
            // Currently unknown
            Assert.EndsWith($"usr/bin", builder.BuildUsrBin);
            Assert.EndsWith($"usr/share", builder.BuildUsrShare);
        }

        Assert.NotEmpty(builder.BuildAppBin);
    }
}