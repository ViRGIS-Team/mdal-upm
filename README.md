[![openupm](https://img.shields.io/npm/v/com.virgis.mdal?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.virgis.mdal/)

# Unity Package for MDAL

The [Mesh Data Abstraction Layer](https://www.mdal.xyz/) (MDAL) is a C++ library for handling unstructured mesh data released with MIT license. It provides a single data model for multiple supported data formats. MDAL is used by QGIS for data access for mesh layers. 

MDAL is from the same "stable" of DALs as [GDAL](https://gdal.org/) and has a dependency upon GDAL.

This repo is a Unity Package for using MDAL in a project.

This Package is part of the [ViRGiS project](https://www.virgis.org/) - bringing GiS to VR. 

## Installation

The Package can be installed from [Open UPM](https://openupm.com/packages/com.virgis.mdal/). If you use this method, the dependencies will be automatically loaded provided the relevant scoped registry is included in your project's `manifest.json` :
```
scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.openupm",
        "com.virgis.geometry3sharp",
        "com.virgis.conda-extensions"
      ]
    }
  ],
```


The Package can also be installed using the Unity Package Manager directly from the [GitHub Repo](https://github.com/ViRGIS-Team/mdal-upm).

The UPM package is based on the Conda package which has dependencies on other Conda packages but that is not apparent to the user.

## Build Tests and Platform Support

This package is build tested on multiple platforms using the [ViRGiS Team Test Project](https://github.com/ViRGIS-Team/test-project).

See that project for working examples and current list of working platforms.

## Version numbers

This package is a wrapper around a C++ library. We want to keep the link to the library version. However, we also need to be able to have multiple
builds of the package for the same underlying library version. Unfortunately, UPM does not have the concept of a build number.

Therefore, this package uses the version numbering proposed by [Favo Yang to solve this](https://medium.com/openupm/how-to-maintain-upm-package-part-3-2d08294269ad#88d8). This adds two digits for build number to the SemVer patch value i.e. 3.1.1 => 3.1.100, 3.1.101, 3.1..102 etc.

This has the unfortunate side effect that 3.1.001 will revert to 3.1.1 and this means :

| Package | Library |
| ------- | ------- |
| 3.1.0   | 3.1.0   |
| 3.1.1   | 3.1.0   |
| 3.1.100 | 3.1.1.  |

## A note about Upgrading

Unity is a bit "graby" about DLLs and SOs. Once it is loaded it keeps a hardlink to the DLL and does not like changing. This means that for this package, once you have upgraded to a new version of the UPM package you will, usually, need to restart the Unity Editor for the change to work.

## Development and Use in the player
> NOTE For the avoidance of doubt, conda is NOT required on machines running the distributed Unity application. The required libraries are automatically included in the distribution package created by Unity.

The scripts for accessing MDAL data are included in the `Mdal`namespace and follow the [MDAL C Api](https://www.mdal.xyz/api/mdal_c_api.html).

For more details - see the documentation.

The MDAL library is loaded as an unmanaged native plugin. This plugin will load correctly in the player when built. See below for a note about use in the Editor.

This Library works on Windows, Mac and Linux platforms.

## Running in the Editor

This package uses [Conda](https://docs.conda.io/en/latest/) to download the latest version of MDAL.

As of version 1.3.2, this package uses Version 2 of the Conda Extension package. This means that the package now includes a complete self contained and standalone installation of the Conda API. You no longer need to install Conda on your development machines.

Note that when upgrading, you MUST delete the Assets/Conda directory and restart Unity.

## Documentation

See [the API Documentation](https://virgis-team.github.io/mdal-upm/html/annotated.html).

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

# Minimum Project / Test Project
You can see a minimum working project in the test project used to test build this package (and two others):

https://github.com/ViRGIS-Team/test-project.

# Use with Unity Cloud Build

As of release 1.3.2, this package will work with Unity Cloud Build without any need for additional work or scripts.