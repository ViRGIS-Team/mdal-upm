[![openupm](https://img.shields.io/npm/v/com.virgis.mdal?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.virgis.mdal/)

# Unity Package for MDAL

The [Mesh Data Abstraction Layer](https://www.mdal.xyz/) (MDAL) s a C++ library for handling unstructured mesh data released with MIT license. It provides a single data model for multiple supported data formats. MDAL is used by QGIS for data access for mesh layers. 

MDAL is from the same "stable"of DALs as [GDAL](https://gdal.org/) and has a dependency upon GDAL.

This repo is a Unity Package for using MDAL in a project.

This Package is part of the [ViRGiS project](https://www.virgis.org/) - bringing GiS to VR. 

## Installation

The Package can be installed from [Open UPM](https://openupm.com/packages/com.virgis.mdal/). If you use this method, the dependencies will be automatically loaded provided the relevent scoped registry is included in your project's `manifest.json` :
```
scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.openupm",
        "com.virgis.geometry3sharp"
      ]
    }
  ],
```


The Package can also be installed using the Unity Package Manager directly from the [GitHub Repo](https://github.com/ViRGIS-Team/mdal-upm).

This package is dependent on the following packages having been loaded (and the UPM / GH combination does not allow package dependencies  - so you have to do that yourself) :

- Geometry3Sharp -[UPM GH Repo](https://github.com/ViRGIS-Team/geometry3Sharp). This providse the Mesh data structure and manipulation tools in c#

The UPM package is based on the Conda package which has depedencies on other Conda packages but that is not apparent to the user.

## Version numbers

This package is a wrapper around a C++ library. We want to keep the link to the library version. However, we also need to be able to have multiple
builds of the package for the same underlying library version. Unfortunately, UPM does not have the concept of a build number.

Therefore, this package uses the version number ing proposed by [Favo Yang to solve this](https://medium.com/openupm/how-to-maintain-upm-package-part-3-2d08294269ad#88d8). This adds two digits for build number to the SemVer patch value i.e. 3.1.1 => 3.1.100, 3.1.101, 3.1..102 etc.

This has the unfortunate side effect that 3.1.001 will revert to 3.1.1 and this means :

| Package | Library |
| ------- | ------- |
| 3.1.0   | 3.1.0   |
| 3.1.1   | 3.1.0   |
| 3.1.100 | 3.1.1.  |
 

## Development and Use in the player

The scripts for accessing MDAL data are included in the `Mdal`namespace and follow the [MDAL C Api](https://www.mdal.xyz/api/mdal_c_api.html).

For more details - see the documentation.

The MDAL library is loaded as an unmanaged native plugin. This plugin will load correctly in the player when built. See below for a note about use in the Editor.

This Library works on Windows, Mac and Linux based platforms.

## Running in the Editor

This package uses [Conda](https://docs.conda.io/en/latest/) to download the latest version of MDAL.

For this package to work , the development machine MUST have a working copy of Conda (either full Conda or Miniconda) installed and in the path. The following CLI command should work without change or embellishment:

```
conda info
```

If the Editor is running on Windows, there must also be a reasonably recent vesion of Powershell installed.

The package will keep the installation of Mdal in `Assets\Conda`. You may want to exclude this folder from source control.

This package installs the GDAL package, which copies data for GDAL and for PROJ into the `Assets/StreamingAssets`folder. You may also want to exclude the sub-folders GDAL and PROJ from source control.

## Documentation

See [the API Documentation](https://virgis-team.github.io/mdal-upm/html/index.html).

A typical sample program :

```c#
using Mdal;
using g3;

  List<DMehs3> features = new List<DMesh3>();

  // for MDAL files - load the mesh directly
  ds = Datasource.Load("...SourceFileName");

  for (int i = 0; i < ds.meshes.Length; i++) {
      DMesh3 mesh = ds.GetMesh(i);
      mesh.RemoveMetadata("properties");
      mesh.AttachMetadata("properties", new Dictionary<string, object>{
      { "Name", ds.meshes[i] }
  });
      // set the CRS based on what is known
      if (proj != null) {
          mesh.RemoveMetadata("CRS");
          mesh.AttachMetadata("CRS", proj);
      }
      if (layer.ContainsKey("Crs") && layer.Crs != null) {
          mesh.RemoveMetadata("CRS");
          mesh.AttachMetadata("CRS", layer.Crs);
      };
      features.Add(mesh);
  }
```
