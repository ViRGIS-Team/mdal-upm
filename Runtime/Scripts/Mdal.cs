using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text;
using g3;
using Microsoft.Win32.SafeHandles;

namespace Mdal {

    /// <summary>
    /// Simple Mdal Config routine - it really just returns the Version of MDAL
    ///
    /// The real use is that if there is a DLL error - this call will trigger it.
    /// </summary>
    public class MdalConfiguration{
        public static string ConfigureMdal() {
            return Mdal.GetVersion();
        }
    }

    /// <summary>
    /// The basic MDAL Instance Object
    /// </summary>
    public class Mdal {

        private const string MDAL_LIBRARY = "mdal";

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_Version")]
        private static extern IntPtr MDAL_Version();

        /// <summary>
        /// Get the MDAL version string
        ///
        /// <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv412MDAL_Versionv">Implements MDAL_Version()</a>
        /// </summary>
        /// <returns> <see cref="string" /> MDAL version string</returns>
        public static string GetVersion() {
            IntPtr ret = MDAL_Version();
            string version = Marshal.PtrToStringAnsi(ret);
            return version;
        }

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_LastStatus")]
        private static extern int MDAL_lastStatus();

        /// <summary>
        /// Get the status message for the last call on MDAL
        ///
        /// <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv415MDAL_LastStatusv">Implements MDAL_LastStatus</a>
        /// </summary>
        /// <returns cref="MDAL_Status"> <see cref="MDAL_Status" />Status </returns>
        public static MDAL_Status LastStatus() {
            int ret = MDAL_lastStatus();
            return (MDAL_Status) ret;
        }

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_LoadMesh")]
        public static extern MdalMesh MDAL_LoadMesh([MarshalAs(UnmanagedType.LPStr)] StringBuilder uri);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_MeshNames")]
        private static extern IntPtr MDAL_MeshNames([MarshalAs(UnmanagedType.LPStr)] StringBuilder uri);

        /// <summary>
        /// Gets the mesh names as URIs
        ///
        /// <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv414MDAL_MeshNamesPKc">Implements MDAL_MeshNames</a>
        /// </summary>
        /// <param name="uri"> <see cref="string" /> The URI for Datasource</param>
        /// <returns>a <see cref ="string" /> containing a list of URIs separated by ;;</returns>
        public static string GetNames(string uri) {
            StringBuilder stb = new(uri);
            IntPtr ret = MDAL_MeshNames(stb);
            return Marshal.PtrToStringAnsi(ret); 
        }

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_vertexCount")]
        public static extern int MDAL_M_vertexCount(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_edgeCount")]
        public static extern int MDAL_M_edgeCount(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_faceCount")]
        public static extern int MDAL_M_faceCount(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_datasetGroupCount")]
        public static extern int MDAL_M_datasetGroupCount(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_projection")]
        private static extern IntPtr MDAL_M_projection(MdalMesh pointer);


        /// <summary>
        /// Get the CRS string for a Mesh
        ///
        /// <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv417MDAL_M_projection10MDAL_MeshH">Implements MDAL_M_projection</a>
        /// </summary>
        /// <param name="pointer"><see cref="MDAL_Status" /> Pointer</param>
        /// <returns> <see cref="string" /> The CRS string</returns>
        public static string GetCRS(MdalMesh pointer) {
            IntPtr ret = MDAL_M_projection(pointer);
            string proj = Marshal.PtrToStringAnsi(ret);
            return proj;
        }

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_vertexIterator")]
        public static extern MdalVertexIterator MDAL_M_vertexIterator(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_VI_next")]
        public static extern int MDAL_VI_next(MdalVertexIterator pointer, int verticesCount,  double[] coordinates);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_VI_close")]
        public static extern void MDAL_VI_close(MdalVertexIterator pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_faceIterator")]
        public static extern MdalFaceIterator MDAL_M_faceIterator(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_FI_next")]
        public static extern int MDAL_FI_next(MdalFaceIterator iterator,
                              int faceOffsetsBufferLen,[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  int[] faceOffsetsBuffer,
                              int vertexIndicesBufferLen,
                              [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] vertexIndicesBuffer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_FI_close")]
        public static extern void MDAL_FI_close(MdalFaceIterator pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_faceVerticesMaximumCount")]
        public static extern int MDAL_M_faceVerticesMaximumCount(MdalMesh pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_M_datasetGroup")]
        public static extern MdalDatasetGroup MDAL_M_datasetGroup(MdalMesh pointer, int index);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_name")]
        public static extern IntPtr MDAL_G_name(MdalDatasetGroup pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_dataLocation")]
        public static extern MDAL_DataLocation MDAL_G_dataLocation(MdalDatasetGroup pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_datasetCount")]
        public static extern int MDAL_G_datasetCount(MdalDatasetGroup pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_dataset")]
        public static extern MdalDataset MDAL_G_dataset(MdalDatasetGroup pointer, int index);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_mesh")]
        public static extern MdalMesh MDAL_G_mesh(MdalDatasetGroup pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_G_hasScalarData")]
        public static extern bool MDAL_G_hasScalarData(MdalDatasetGroup pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_D_valueCount")]
        public static extern int MDAL_D_valueCount(MdalDataset pointer);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_D_data")]
        public static extern int MDAL_D_dataAsDouble(MdalDataset pointer, int start, int count, MDAL_DataType type, double[] values);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_D_data")]
        public static extern int MDAL_D_dataAsInt(MdalDataset pointer, int start, int count, MDAL_DataType type, int[] values);

        [DllImport(MDAL_LIBRARY, EntryPoint = "MDAL_D_group")]
        public static extern MdalDatasetGroup MDAL_D_group(MdalDataset pointer);

    }

    /// <summary>
    /// An Instance of an MDAL Datasource
    /// </summary>
    public class Datasource {

        public string[] meshes;

        public Datasource() {

        }

        /// <summary>
        /// Create a Datasource from a Uri - e.g. filename
        /// </summary>
        /// <param name="uri"><see cref="string" /> Datasource uri </param>
        /// <returns><see cref="Datasource" /></returns>
        public static Datasource Load(string uri)
        {
            Datasource ds = new();
            string ret = Mdal.GetNames(uri);
            MDAL_Status status = Mdal.LastStatus();
            if (ret == null && status != MDAL_Status.None)
                throw new Exception(status.ToString() + uri);
            ds.meshes = ret.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return ds;
        }

        /// <summary>
        /// Returns a <see cref="Task" /> running <see cref="Datasource.Load(string)" />
        /// </summary>
        /// <param name="uri"><see cref="string" /> Datasource uri </param>
        /// <returns><see cref="Task" /> running <see cref="Datasource.Load(string)" /> </returns>
        public static Task<Datasource> LoadAsync(string uri)
        {
            Task<Datasource> t1 = new(() =>
            {
                return Datasource.Load(uri);
            });
            t1.Start();
            return t1;
        }

        /// <summary>
        /// Get a <see cref="MdalMesh" /> object from the <see cref="Datasource" />
        /// </summary>
        /// <param name="index"><see cref="int" /> Index of the Mesh in the Datasource</param>
        /// <returns><see cref="MdalMesh" /></returns>
        public MdalMesh GetMesh(int index) {
            StringBuilder stb = new(meshes[index]);
            return Mdal.MDAL_LoadMesh(stb);
        }

        /// <summary>
        /// Returns a <see cref="Task" /> running <see cref="Datasource.GetMesh(int)" />
        /// </summary>
        /// <param name="index"><see cref="int"/> Index of the Mesh in the Datasource</param>
        /// <returns><see cref="Task" /> running <see cref="Datasource.GetMesh(int)" /></returns>
        public Task<MdalMesh> GetMeshAsync(int index)
        {
            Task<MdalMesh> t1 = new(() => {
                return GetMesh(index);
            });
            t1.Start();
            return t1;
        }

    }

    /// <summary>
    /// Wrapper object for an MDAL Mesh instance
    ///
    /// MdalMesh will implictly cast into either <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.SimpleMesh.html">Simplemesh</a>
    /// or <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.DMesh3.html">DMesh3</a>
    /// </summary>
    public sealed class MdalMesh : SafeHandleZeroOrMinusOneIsInvalid {

        public MdalMesh() : base(ownsHandle: true) {

        }

        protected override bool ReleaseHandle() {
            return true;
        }

        public int GetVertexCount() {
            return Mdal.MDAL_M_vertexCount(this);
        }

        public int GetFaceCount() {
            return Mdal.MDAL_M_faceCount(this);
        }

        public int GetEdgeCount(){
            return Mdal.MDAL_M_edgeCount(this);
        }

        public int GetGroupCount() {
            return Mdal.MDAL_M_datasetGroupCount(this);
        }

        public MdalDatasetGroup GetGroup(int index) {
            return Mdal.MDAL_M_datasetGroup(this, index);
        }

        /// <summary>
        /// The Basic Mesh Creation. Turns an MDAL mesh into a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.SimpleMesh.html">Simplemesh</a>
        /// </summary>
        /// <returns></returns>
        private SimpleMesh ToMesh() {
            int vcount = GetVertexCount();
            MdalVertexIterator vi = Mdal.MDAL_M_vertexIterator(this);
            VectorArray3d v = GetVertexes(vi, vcount);
            vi.Dispose();
            VectorArray3f c = GetColors(vcount, out bool hasColors);
            SimpleMesh mesh = new();
            mesh.AppendVertices(v, null, c);
            int fcount = GetFaceCount();
            MdalFaceIterator fi = Mdal.MDAL_M_faceIterator(this);
            IndexArray3i tri = GetTris(fi, fcount, Mdal.MDAL_M_faceVerticesMaximumCount(this));
            fi.Dispose();
            mesh.AppendTriangles(tri);
            return mesh;
        }

        /// <summary>
        /// Fetch the Colors for an MDAL mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.VectorArray3f.html">VectorArray3f</a>
        /// </summary>
        /// <param name="count"></param>
        /// <param name="hasColors"></param>
        /// <returns></returns>
        VectorArray3f GetColors(int count, out bool hasColors) {
            hasColors = false;
            int dgCount = GetGroupCount();
            if (dgCount == 0)
                return default;
            float[] colors = new float[count * 3];
            double[] red = new double[count];
            double[] green = new double[count];
            double[] blue = new double[count];
            for (int i = 0; i < dgCount; i++) {
                MdalDatasetGroup dg = GetGroup(i);
                string name = dg.GetName().ToLower();
                dg.GetDatasets();
                if (name.StartsWith("r") && dg.datasets.Count > 0) {
                    dg.datasets[0].GetValuesAsScalar(ref red);
                    hasColors = true;
                }
                if (name.StartsWith("g") && dg.datasets.Count > 0)
                    dg.datasets[0].GetValuesAsScalar(ref green);
                if (name.StartsWith("b") && dg.datasets.Count > 0)
                    dg.datasets[0].GetValuesAsScalar(ref blue);
            }
            if (hasColors) {
                for (int i = 0; i < count; i++) {
                    colors[i * 3] = (float) (red[i] / 255);
                    colors[i * 3 + 1] = (float) (green[i] / 255);
                    colors[i * 3 + 2] = (float) (blue[i] / 255);
                }
            }
            return new VectorArray3f(colors);
        }

        /// <summary>
        /// Fetch an MDAL Mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.DMesh3.html">DMesh3</a> with MetaData
        /// </summary>
        /// <returns></returns>
        private DMesh3 ToDMesh() {
            DMesh3 mesh = new(ToMesh(), MeshHints.None, MeshComponents.VertexColors);
            string CRS = Mdal.GetCRS(this);
            if (CRS != null)
                mesh.AttachMetadata("CRS", CRS);
            return mesh;
        }

        /// <summary>
        /// Returns the mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.SimpleMesh.html">Simplemesh</a>
        /// 
        /// </summary>
        /// <returns><a href="https://virgis-team.github.io/geometry3Sharp/api/g3.SimpleMesh.html">Simplemesh</a></returns>
        public static implicit operator SimpleMesh(MdalMesh thisMesh) => thisMesh.ToMesh();

        /// <summary>
        /// Returns the mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.DMesh3.html">DMesh3</a>
        ///
        /// </summary>
        /// <returns><a href="https://virgis-team.github.io/geometry3Sharp/api/g3.DMesh3.html">DMesh3</a></returns>
        public static implicit operator DMesh3(MdalMesh thisMesh) => thisMesh.ToDMesh();

        /// <summary>
        /// Get the verteces of the mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.VectorArray3d.html">VectorArray3d</a>
        /// </summary>
        /// <param name="count"><see cref="int"/> int number of verteces to fetch</param>
        /// <returns><a href="https://virgis-team.github.io/geometry3Sharp/api/g3.VectorArray3d.html">VectorArray3d</a></returns>
        public VectorArray3d GetVertexes(MdalVertexIterator vi, int count)
        {
            double[] vertexes = new double[count * 3];
            Mdal.MDAL_VI_next(vi, count, vertexes);
            return new VectorArray3d(vertexes);
        }

        /// <summary>
        /// Get the faces of the mesh as a <a href="https://virgis-team.github.io/geometry3Sharp/api/g3.IndexArray3i.html">IndexArray3i</a>
        /// </summary>
        /// <remarks> This method can currently only process meshes of triangles or quads</remarks>
        /// <param name="count"><see cref="int"> Number of faces to fetch </param>
        /// <param name="faceSize"><see cref="int"/> Largest Face size</param>
        /// <returns><a href="https://virgis-team.github.io/geometry3Sharp/api/g3.IndexArray3i.html">IndexArray3i</a></returns>
        public IndexArray3i GetTris(MdalFaceIterator fi, int count, int faceSize)
        {
            if (faceSize > 4)
                throw new NotSupportedException("Only Tri and Quad Meshes supported");
            int[] faces;
            faces = GetFaces(fi, count, faceSize, out int[] faceOff);
            if (faceSize == 4)
            {
                List<int> ret = new();
                int previous = 0;
                for (int i = 0; i < count; i++)
                {
                    int current = faceOff[i];
                    int size = current - previous;
                    ret.Add(faces[previous]);
                    ret.Add(faces[previous + 1]);
                    ret.Add(faces[previous + 2]);
                    if (size == 4)
                    {
                        ret.Add(faces[previous + 2]);
                        ret.Add(faces[previous + 3]);
                        ret.Add(faces[previous]);
                    }
                    previous = current;
                }
                return new IndexArray3i(ret.ToArray());
            }
            return new IndexArray3i(faces);
        }

        /// <summary>
        /// Get faces from the face iterator as int[]
        /// </summary>
        /// <param name="count"> number of faces to fetch</param>
        /// <param name="faceSize"> maximum size of face - should be the result from MDAL_M_faceVerticesMaximumCount</param>
        /// <param name="faceOff"> array for the face offsets</param>
        /// <returns>int[] of the faces - will be sparse</returns>
        public int[] GetFaces(MdalFaceIterator fi, int count, int faceSize, out int[] faceOff)
        {
            int[] faces = new int[count * faceSize];
            faceOff = new int[count];
            Mdal.MDAL_FI_next(fi, count, faceOff, count * faceSize, faces);
            return faces;
        }
    }

    /// <summary>
    /// Wrapper for an instance of <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv424MDAL_MeshVertexIteratorH"> MDAL_MeshVertexIteratorH </a>
    /// </summary>
    public sealed class MdalVertexIterator : SafeHandleZeroOrMinusOneIsInvalid {
        
        public MdalVertexIterator() : base (ownsHandle: true) {

        }

        public new void Dispose()
        {
            Mdal.MDAL_VI_close(this);
            base.Dispose();
        }

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }

    /// <summary>
    /// Wrapper for an instance of <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv422MDAL_MeshFaceIteratorH">MDAL_MeshFaceIteratorH</a>
    /// </summary>
    public sealed class MdalFaceIterator : SafeHandleZeroOrMinusOneIsInvalid
    {
        public MdalFaceIterator() : base(ownsHandle: true)
        {

        }

        public new void Dispose()
        {
            Mdal.MDAL_FI_close(this);
            base.Dispose();
        }

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }

    /// <summary>
    /// Wrapper for an instance of <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv418MDAL_DatasetGroupH">MDAL_DatasetGroupH</a>
    /// </summary>
    public sealed class MdalDatasetGroup : SafeHandleZeroOrMinusOneIsInvalid {
        public List<MdalDataset> datasets = new();

        public MdalDatasetGroup() : base(ownsHandle: true) {

        }

        protected override bool ReleaseHandle() {
            return true;
        }

        /// <summary>
        /// Returns the DatasetGroup Name
        ///
        /// Implements <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv411MDAL_G_name18MDAL_DatasetGroupH">MDAL_G_name</a>
        /// </summary>
        /// <returns><see cref="string"/> name</returns>
        public string GetName() {
            IntPtr ret = Mdal.MDAL_G_name(this);
            return Marshal.PtrToStringAnsi(ret);
        }

        /// <summary>
        /// Fetches the member <see cref="MdalDataset" />s from the Group and stores them
        /// in the <see cref="MdalDatasetGroup.datasets"/> member
        ///
        /// Implements <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv414MDAL_G_dataset18MDAL_DatasetGroupHi">MDAL_G_dataset</a>
        /// </summary>
        public void GetDatasets() {
            int count = Mdal.MDAL_G_datasetCount(this);
            for (int i = 0; i < count; i++) {
                datasets.Add(Mdal.MDAL_G_dataset(this, i));
            }
        }

        public MDAL_DataLocation GetLocation() {
            return Mdal.MDAL_G_dataLocation(this);
        }

        public bool IsScalar() {
            return Mdal.MDAL_G_hasScalarData(this);
        }

        public MdalMesh GetMesh() {
            return Mdal.MDAL_G_mesh(this);
        }
    }

    /// <summary>
    /// Class that repersents an instance of a voxel. It has a simple primative shape as vertices and a color
    /// </summary>
    public sealed class Voxel {

        public Colorf color;
        public List<Vector3d> vertices;
    
        public Voxel CreateNewGridElement(bool bCopy)
        {
            Voxel ret = new();
            if (bCopy)
            {
                ret.color = color;
                ret.vertices = vertices;
            }
            return ret;
        }
    }

    /// <summary>
    /// Class that represents an instance of a VoxelMesh.
    /// 
    /// It is an IEnumerable that contains a list of Voxels
    /// 
    /// The voxels can be addressed as a 2D matrix of columes (i.e. faces of a 2.xd Mesh) and rows (i.e. vertical extrusions)
    /// 
    /// The rows can be arbitrary lengths (and indeed shape)
    /// </summary>
    public sealed class VoxelMesh: IEnumerable {
        readonly List<Voxel> m_Voxels;
        readonly List<int> colIndices;
        public Dictionary<string, string> MetaData;

        public VoxelMesh(){
            m_Voxels = new List<Voxel>();
            colIndices = new List<int>();
            MetaData = new Dictionary<string, string>();
        }

        public int Count { get { return m_Voxels.Count; } }

        public IEnumerator GetEnumerator() {
            return m_Voxels.GetEnumerator();
        }

        public int AddRow(int size){
            colIndices.Add(m_Voxels.Count);
            for (int i = 0; i<size; i++)
            {
                m_Voxels.Add(new Voxel());
            }
            return colIndices.Count - 1;
        }

        public Voxel Get(int row, int col){
            return m_Voxels[colIndices[row] + col];
        }
    }

    /// <summary>
    /// Wrapper for an instance of <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv413MDAL_DatasetH">MDAL_DatasetH</a>
    /// </summary>
    public sealed class MdalDataset : SafeHandleZeroOrMinusOneIsInvalid {

        public MdalDataset() : base(ownsHandle: true) {

        }

        protected override bool ReleaseHandle() {
            return true;
        }

        public MdalDatasetGroup GetGroup(){
            return Mdal.MDAL_D_group(this);
        }

        public int ValueCount() {
            return Mdal.MDAL_D_valueCount(this);
        }

        /// <summary>
        /// Returns the values in the <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv413MDAL_DatasetH">MDAL_DatasetH</a>
        ///
        /// Implements <a href="https://www.mdal.xyz/api/mdal_c_api.html#_CPPv411MDAL_D_data13MDAL_DatasetHii13MDAL_DataTypePv">MDAL_D_data</a>
        /// </summary>
        /// <param name="values"> Array to put the values in</param>
        public void GetValuesAsScalar(ref double[] values) {
            if (values.Length > ValueCount() )
                throw new Exception("invalid count value", new ArgumentException());
            Mdal.MDAL_D_dataAsDouble(this, 0, values.Length, MDAL_DataType.SCALAR_DOUBLE, values);
        }

        public void GetValuesAsVector(ref double[] values)
        {
            if (values.Length > ValueCount() * 2)
                throw new Exception("invalid count value", new ArgumentException()); ;
            Mdal.MDAL_D_dataAsDouble(this, 0, values.Length, MDAL_DataType.VECTOR_2D_DOUBLE, values);
        }

        public void GetValuesAsLevelCount(ref int[] values)
        {
            if (values.Length > GetGroup().GetMesh().GetFaceCount())
                throw new Exception("invalid count value", new ArgumentException()); ;
            Mdal.MDAL_D_dataAsInt(this, 0, values.Length, MDAL_DataType.VECTOR_2D_DOUBLE, values);
        }

        public void GetValuesAsLevelValues(ref double[] values)
        {
            if (values.Length > GetGroup().GetMesh().GetFaceCount() + ValueCount() )
                throw new Exception("invalid count value", new ArgumentException()); ;
            Mdal.MDAL_D_dataAsDouble(this, 0, values.Length, MDAL_DataType.VECTOR_2D_DOUBLE, values);
        }

        public void GetValuesAsFaceIndices(ref int[] values)
        {
            if (values.Length > GetGroup().GetMesh().GetFaceCount() )
                throw new Exception("invalid count value", new ArgumentException()); ;
            Mdal.MDAL_D_dataAsInt(this, 0, values.Length, MDAL_DataType.VECTOR_2D_DOUBLE, values);
        }

        public void GetValuesAsScalar3D(ref double[] values)
        {
            if (values.Length > ValueCount() )
                throw new Exception("invalid count value", new ArgumentException());
            Mdal.MDAL_D_dataAsDouble(this, 0, values.Length, MDAL_DataType.SCALAR_VOLUMES_DOUBLE, values);
        }

        public void GetValuesAsVector3D(ref double[] values)
        {
            if (values.Length > ValueCount() * 2)
                throw new Exception("invalid count value", new ArgumentException()); ;
            Mdal.MDAL_D_dataAsDouble(this, 0, values.Length, MDAL_DataType.VECTOR_2D_VOLUMES_DOUBLE, values);
        }

        public Task<VoxelMesh> ToVoxelMeshAsync()
        {
            Task<VoxelMesh> t1 = new(() => { return ToVoxelMesh(); });
            t1.Start();
            return t1;
        }

        /// <summary>
        /// Converts a DataAsVolume Dataset to a VoxelMesh
        /// </summary>
        /// <returns></returns>
        public VoxelMesh ToVoxelMesh()
        {
            if (GetGroup().GetLocation() != MDAL_DataLocation.DataOnVolumes)
            {
                throw new Exception($"{GetGroup().GetName()} is not a Volumetric dataset", new ArgumentException());
            }

            VoxelMesh vmesh = new();

            // Get the total volume count

            int volCount = ValueCount();
            MdalMesh mesh = GetGroup().GetMesh();
            int fcount = mesh.GetFaceCount();
            int vcount = mesh.GetVertexCount();

            vmesh.MetaData["CRS"] = Mdal.GetCRS(mesh);

            // Get the structure

            int[] levelCounts = new int[fcount];
            GetValuesAsLevelCount(ref levelCounts);

            double[] levelValues = new double[volCount + fcount];
            GetValuesAsLevelValues(ref levelValues);

            int[] faceIndices = new int[fcount];
            GetValuesAsFaceIndices(ref faceIndices);

            MdalVertexIterator vi = Mdal.MDAL_M_vertexIterator(mesh);
            VectorArray3d vertices = mesh.GetVertexes(vi, vcount);
            MdalFaceIterator fi = Mdal.MDAL_M_faceIterator(mesh);
            int[] faces = mesh.GetFaces(fi, fcount, Mdal.MDAL_M_faceVerticesMaximumCount(mesh), out int[] faceOff);


            // Get the data

            double[] data = new double[volCount];

            if (GetGroup().IsScalar()) {
                GetValuesAsScalar3D(ref data);
            } else {
                // Vector data
                data = new double[volCount * 2];
                GetValuesAsVector3D(ref data);
            }

            // Create the Voxels

            for( int i = 0; i < fcount; i++)
            {
                int offset = faceOff[ i ];
                int size = faceOff[i + 1] - offset;
                vmesh.AddRow(levelCounts[i]);
                for ( int j = 0; j < levelCounts[i]; j++)
                {
                    Voxel voxel = vmesh.Get(i,j);

                    // create the vertex list
                    List<Vector3d> vlist = new List<Vector3d>();
                    double l0 = levelValues[faceIndices[i] + j];
                    double l1 = levelValues[faceIndices[i] + j + 1];

                    for ( int k = 0; k < size; k++)
                    {
                        Vector3d vertex = vertices[faces[offset + k]];
                        vertex.z = (float)l0;
                        vlist.Add(vertex);
                    }

                    for (int k = 0; k < size; k++)
                    {
                        Vector3d vertex = vertices[faces[offset + k]];
                        vertex.z = (float)l0;
                        vlist.Add(vertex);
                    }
                    voxel.color = Colorf.DarkBlue;
                    voxel.vertices = vlist;
                }
            }

            return vmesh;
        }
    }

    /// <summary>
    /// The Status Codes for MDAL
    /// </summary>
    public enum MDAL_Status {
        None,
        Err_NotEnoughMemory,
        Err_FileNotFound,
        Err_UnknownFormat,
        Err_IncompatibleMesh,
        Err_InvalidData,
        Err_IncompatibleDataset,
        Err_IncompatibleDatasetGroup,
        Err_MissingDriver,
        Err_MissingDriverCapability,
        Err_FailToWriteToDisk,
        Err_UnsupportedElement,
        Warn_InvalidElements,
        Warn_ElementWithInvalidNode,
        Warn_ElementNotUnique,
        Warn_NodeNotUnique,
        Warn_MultipleMeshesInFile,
    }

    /// <summary>
    /// MDAL Log Levwlas
    /// </summary>
    public enum MDAL_LogLevel {
        Error,
        Warn,
        Info,
        Debug
    }

    /// <summary>
    /// Specifies where the data is defined.
    /// </summary>
    public enum MDAL_DataLocation {
        DataInvalidLocation, //Unknown/Invalid location.
        DataOnVertices, //Data is defined on vertices of 1D or 2D mesh.
        DataOnFaces,  //Data is defined on face centres of 2D mesh.
        DataOnVolumes, //Data is defined on volume centres of 3D mesh.
        DataOnEdges,  //Data is defined on edges of 1D mesh.
    }

    /// <summary>
    /// Data type to be returned by MDAL_D_data.
    /// </summary>
    public enum MDAL_DataType {
        SCALAR_DOUBLE,  //Double value for scalar datasets (DataOnVertices or DataOnFaces or DataOnEdges)
        VECTOR_2D_DOUBLE, //Double, double value for vector datasets(DataOnVertices or DataOnFaces or DataOnEdges)
        ACTIVE_INTEGER,  //Integer, active flag for dataset faces.Some formats support switching off the element for particular timestep (see MDAL_D_hasActiveFlagCapability)
        VERTICAL_LEVEL_COUNT_INTEGER, //Number of vertical level for particular mesh’s face in 3D Stacked Meshes (DataOnVolumes)
        VERTICAL_LEVEL_DOUBLE,  //Vertical level extrusion for particular mesh’s face in 3D Stacked Meshes (DataOnVolumes)
        FACE_INDEX_TO_VOLUME_INDEX_INTEGER, //The first index of 3D volume for particular mesh’s face in 3D Stacked Meshes (DataOnVolumes)
        SCALAR_VOLUMES_DOUBLE, //Double scalar values for volumes in 3D Stacked Meshes (DataOnVolumes)
        VECTOR_2D_VOLUMES_DOUBLE, //Double, double value for volumes in 3D Stacked Meshes (DataOnVolumes)
    }
}
