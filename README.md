# Unity Package for MDAL

The [Mesh Data Abstraction Layer](https://www.mdal.xyz/) (MDAL) s a C++ library for handling unstructured mesh data released with MIT license. It provides a single data model for multiple supported data formats. MDAL is used by QGIS for data access for mesh layers. 

MDAL is from the same "stable"of DALs as [GDAL](https://gdal.org/) and has a dependency upon GDAL.

This repo is a Unity Package for using MDAL in a project.

This Package is part of the [ViRGiS project](https://www.virgis.org/) - bringing GiS to VR. 

## Installation

The Package can be installed using the Unity Package Manager directly from the [GitHub Repo](https://github.com/ViRGIS-Team/mdal-upm).

This package is dependent on the following packages having been loaded (and the UPM / GH combination does not allow package dependencies  - so you have to do that yourself) :

- Geometry3Sharp -[UPM GH Repo](https://github.com/ViRGIS-Team/geometry3Sharp). This providse the Mesh data strcuture and manipulation tools in c#, and

- GDAL - [UPM GH repo](https://github.com/ViRGIS-Team/gdal-upm).

## Developement and Use in the player

The scripts for accessing MDAL data are included in the `Mdal`namespace and follow the [MDAL C Api](https://www.mdal.xyz/api/mdal_c_api.html).

For more details - see the documentation.

The MDAL library is loaded as an unmanaged native plugin. This plugin will load correctly in the player when built. See below for a note about use in the Editor.

This Library ONLY works on Windows.

## Running in the Editor

The native plugins will NOT work out of the box in the Editor.

This is because of some problems with the DLL search paths and the directory structure used by Unity for packages - see more discussion [here](https://forum.unity.com/threads/plugins-and-resources-inside-package.730328/#post-6432950).

The fix is tro create a copy of the Plugins folder from the  [GDAL Package](https://github.com/ViRGIS-Team/gdal-upm) on your development machine outside of the Unity Project folder structure and add this to the system Path.

I usually do this by creating a clone of the GDAL package on my development machine and adding this to the path (noting that you need to add the Plugins folder - i.e. the location of the DLLs - and not the root folder to the path!). 

## Documentation

TBD