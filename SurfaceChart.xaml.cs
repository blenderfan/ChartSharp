using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChartSharp
{

    /* Class: SurfaceChart
    
       A WPF Chart Control, that can render surface data in different styles. It has far less features than similar software, but is (maybe) easy to setup and use. 
    */
    [ContentProperty("Items")]
    public partial class SurfaceChart : UserControl
    {

        /* Constructor: SurfaceChart
         
           Initializes properties to their default values, manages event handlers and
           initializes control components.
        */
        public SurfaceChart()
        {
            ChartStyle = new Chart3DStyle();
            MinX = 0;
            MinY = 0;
            MinZ = 0;
            MaxX = 1;
            MaxY = 1;
            MaxZ = 1;
            Title = null;
            XLabel = null;
            YLabel = null;
            ZLabel = null;

            CameraRotY = Math.PI / 2.0f;

            Loaded += new RoutedEventHandler(OnLoaded);

            InitializeComponent();

        }

        #region Properties

        #region UIProperties

        private TextBlock TitleBlock { get; set; } = new TextBlock();
        private TextBlock XLabelBlock { get; set; } = new TextBlock();
        private TextBlock YLabelBlock { get; set; } = new TextBlock();
        private TextBlock ZLabelBlock { get; set; } = new TextBlock();
        private Rectangle ViewportBackground { get; set; } = new Rectangle();
        private Border GraphBorder { get; set; } = new Border();
        private List<TextBlock> Numbers { get; set; } = new List<TextBlock>();

        private List<UIElement> CanvasElements { get; set; } = new List<UIElement>();

        private Viewport3D Viewport { get; set; } = new Viewport3D();
        private PerspectiveCamera Camera { get; set; } = new PerspectiveCamera();
        private Model3DGroup Frame { get; set; } = new Model3DGroup();
        private Model3DGroup Surface { get; set; } = new Model3DGroup();
        private Model3DGroup XAxisMesh { get; set; } = new Model3DGroup();
        private Model3DGroup YAxisMesh { get; set; } = new Model3DGroup();
        private Model3DGroup ZAxisMesh { get; set; } = new Model3DGroup();
        private Model3DGroup Grid { get; set; } = new Model3DGroup();
        private Model3DGroup Light { get; set; } = new Model3DGroup();
        private Model3DGroup Markings { get; set; } = new Model3DGroup();


        private Model3DGroup ViewportElements { get; set; } = new Model3DGroup();

        private List<SurfaceData> Series { get; set; } = new List<SurfaceData>();
        private List<SurfaceData> ViewSeries { get; set; } = new List<SurfaceData>();


        #endregion

        #region InternalProperties

        private double MarginYTop { get; set; }
        private double MarginYBottom { get; set; }
        private double MarginXLeft { get; set; }
        private double MarginXRight { get; set; }
        private double CenterX { get; set; }
        private double CenterY { get; set; }

        private Point3D Position { get; set; }
        private Vector3D LookDirection { get; set; }

        private bool Rotate { get; set; }
        private Point MoveStart { get; set; }
        private double CameraRotY { get; set; }
        private double CameraRotUp { get; set; }
        private double CameraDistance { get; set; } = 10;

        #endregion

        #region DependencyProperties


        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(IEnumerable), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty XLabelProperty = DependencyProperty.Register("XLabel", typeof(string), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty YLabelProperty = DependencyProperty.Register("YLabel", typeof(string), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ZLabelProperty = DependencyProperty.Register("ZLabel", typeof(string), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinXProperty = DependencyProperty.Register("MinX", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MinYProperty = DependencyProperty.Register("MinY", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxXProperty = DependencyProperty.Register("MaxX", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxYProperty = DependencyProperty.Register("MaxY", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MinZProperty = DependencyProperty.Register("MinZ", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxZProperty = DependencyProperty.Register("MaxZ", typeof(double), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ChartStyleProperty = DependencyProperty.Register("ChartStyle", typeof(Chart3DStyle), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(Chart3DStyle), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeatStartProperty = DependencyProperty.Register("HeatStart", typeof(Color?), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(Color?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty HeatEndProeprty = DependencyProperty.Register("HeatEnd", typeof(Color?), typeof(SurfaceChart), new FrameworkPropertyMetadata(default(Color?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /* Property: ChartStyle
        
           Defines the style of the chart. Includes color, style of the X-, Y- and Z-Axis and style of a background grid.

           Default Value:

           Constructor of <ChartSharp.Chart3DStyle>
        */
        public Chart3DStyle ChartStyle
        {
            get { return (Chart3DStyle)GetValue(ChartStyleProperty); }
            set { SetValue(ChartStyleProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /* Property: MaxX
        
           The maximal value in the X-Direction. If MaxX <= MinX, an ArgumentException occurs.

           Default Value:

           1
        */
        public double MaxX
        {
            get { return (double)GetValue(MaxXProperty); }
            set { SetValue(MaxXProperty, value); }
        }

        /* Property: MaxY
        
           The maximal value in the Y-Direction. If MaxY <= MinY, an ArgumentException occurs.

           Default Value:

           1
        */
        public double MaxY
        {
            get { return (double)GetValue(MaxYProperty); }
            set { SetValue(MaxYProperty, value); }
        }

        /* Property: MaxZ
        
           The maximal value in the Z-Direction. If MaxZ <= MinZ, an ArgumentException occurs.

           Default Value:

           1
        */
        public double MaxZ
        {
            get { return (double)GetValue(MaxZProperty); }
            set { SetValue(MaxZProperty, value); }
        }

        /* Property: MinX
        
           The minimal value in the X-Direction. If MinX >= MaxX, an ArgumentException occurs.

           Default Value:

           0
        */
        public double MinX
        {
            get { return (double)GetValue(MinXProperty); }
            set { SetValue(MinXProperty, value); }
        }

        /* Property: MinY
        
           The minimal value in the Y-Direction. If MinY >= MaxY, an ArgumentException occurs.

           Default Value:

           0
        */
        public double MinY
        {
            get { return (double)GetValue(MinYProperty); }
            set { SetValue(MinYProperty, value); }
        }

        /* Property: MinZ
        
           The minimal value in the X-Direction. If MinZ >= MaxZ, an ArgumentException occurs.

           Default Value:

           0
        */
        public double MinZ
        {
            get { return (double)GetValue(MinZProperty); }
            set { SetValue(MinZProperty, value); }
        }

        /* Property: Title
         
           The title of the <SurfaceChart>. It is displayed above the chart and is centered. If null, no text will be displayed.

           Default Value:

           null
        */
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /* Property: XLabel
        
           The label of the X-Axis. It is only displayed if XLabel is not null and <ChartStyle.XAxisStyle> is not null.

           Default Value:

           null
        */
        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        /* Property: YLabel
        
           The label of the Y-Axis. It is only displayed if YLabel is not null and <ChartStyle.YAxisStyle> is not null.

           Default Value:

           null
        */
        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        /* Property: ZLabel
        
           The label of the Z-Axis. It is only displayed if ZLabel is not null and <ChartStyle.ZAxisStyle> is not null.

           Default Value:

           null
        */
        public string ZLabel
        {
            get { return (string)GetValue(ZLabelProperty); }
            set { SetValue(ZLabelProperty, value); }
        }

        public ItemCollection Items { get { return itemsControl.Items; } }


        #endregion

        private enum Plane
        {
            XY, XZ, YZ
        }

        private enum Direction
        {
            X, Y, Z
        }

        #endregion

        #region Helper Functions

        //Maybe I should have used Matrices after all...
        //Well, as long as there are no performance issues...
        private Point? Point3DToScreenCoords(Viewport3D viewport, Point3D point)
        {
            Point? screenPoint = null;

            var camera = (PerspectiveCamera)viewport.Camera;
            var width = viewport.Width;
            var height = viewport.Height;
            var transform = camera.Transform;
            var position = transform.Transform(camera.Position);
            var lookDirection = transform.Transform(camera.LookDirection);
            lookDirection.Normalize();
            var upDirection = transform.Transform(camera.UpDirection);
            upDirection.Normalize();

            var fov = camera.FieldOfView;
            var near = camera.NearPlaneDistance;

            var aspectRatio = width / height;

            var planeAngle = 180 - (90 + (fov / 2.0f));
            planeAngle *= (Math.PI * 2) / 360.0f;
            fov *= (Math.PI * 2) / 360.0f;

            var halfPlaneLength = (near * Math.Sin(fov / 2.0f)) / Math.Sin(planeAngle);
            var planeLength = 2 * halfPlaneLength;
            var planeHeight = planeLength / aspectRatio;
            var halfPlaneHeight = planeHeight / 2.0f;

            var sideDirection = Vector3D.CrossProduct(lookDirection, upDirection);
            sideDirection.Normalize();

            var centerPoint = position + near * lookDirection;

            var topLeft = centerPoint - halfPlaneLength * sideDirection + halfPlaneHeight * upDirection;
            var topRight = topLeft + planeLength * sideDirection;
            var bottomLeft = topLeft - planeHeight * upDirection;

            var top = topRight - topLeft;
            var left = bottomLeft - topLeft;

            var dir = position - point;

            if (Vector3D.DotProduct(dir, lookDirection) != 0.0f)
            {
                var dist = Vector3D.DotProduct((centerPoint - point), lookDirection) / (Vector3D.DotProduct(dir, lookDirection));
                var planePoint = point + dist * dir;

                var planeVec = planePoint - topLeft;

                top.Normalize();
                left.Normalize();
                double screenX = Vector3D.DotProduct(planeVec, top) / planeLength;
                double screenY = Vector3D.DotProduct(planeVec, left) / planeHeight;
                screenX *= width;
                screenY *= height;

                screenPoint = new Point(screenX, screenY);
            }
            return screenPoint;
        }

        private void ClampPointCollection(double[,] collection)
        {
            for(int i = 0; i < collection.GetLength(0); i++)
                for(int j = 0; j < collection.GetLength(1); j++)
                {
                    if (collection[i, j] > MaxY)
                        collection[i, j] = MaxY;
                    else if (collection[i, j] < MinY)
                        collection[i, j] = MinY;
                }
        }

        private void NormalizePointCollection(double[,] collection)
        {
            double factorY = ChartStyle.SizeY / (MaxY - MinY);
            for(int i = 0; i < collection.GetLength(0); i++)
                for(int j = 0; j < collection.GetLength(1); j++)
                    collection[i, j] *= factorY;
        }

        private void AdjustViewSeries()
        {
            ViewSeries.Clear();
            foreach (var series in Series)
            {
                var originalColl = series.Points;
                double[,] viewColl = new double[originalColl.GetLength(0), originalColl.GetLength(1)];

                Array.Copy(originalColl, viewColl, originalColl.GetLength(0) * originalColl.GetLength(1));
                ClampPointCollection(viewColl);
                NormalizePointCollection(viewColl);

                SurfaceData viewSeries = new SurfaceData
                {
                    DrawStyle = series.DrawStyle,
                    Points = viewColl,
                    Outline = series.Outline,
                    Fill = series.Fill,
                    Thickness = series.Thickness
                };
                ViewSeries.Add(viewSeries);
            }
        }

        #endregion

        #region Visuals

        private void AddTriangle(Int32Collection collection, int idx1, int idx2, int idx3)
        {
            collection.Add(idx1); collection.Add(idx2); collection.Add(idx3);
        }

        private void AddCircle(Int32Collection collection, List<int> vertices)
        {
            int idx = 0;
            while (vertices.Count() > 2)
            {
                int count = vertices.Count();
                AddTriangle(collection, vertices[(idx + 2) % count], vertices[(idx + 1) % count], vertices[idx % count]);
                vertices.Remove(vertices[(idx + 1) % count]);
                idx++;
            }
        }

        private void AddLine(Point3DCollection collection, Int32Collection triangleIndices, Point3D start, Point3D end, double thickness)
        {
            var dir = end - start;
            dir.Y = 0;
            dir.Normalize();

            Vector3D orthogonal = new Vector3D(dir.Z, 0, dir.X);

            Point3D startLeft = start + orthogonal * (thickness / 2.0f);
            Point3D startRight = start - orthogonal * (thickness / 2.0f);
            Point3D endLeft = end + orthogonal * (thickness / 2.0f);
            Point3D endRight = end - orthogonal * (thickness / 2.0f);

            int idx1, idx2, idx3, idx4;

            collection.Add(startLeft);
            idx1 = collection.Count - 1;
            collection.Add(startRight);
            idx2 = collection.Count - 1;
            collection.Add(endLeft);
            idx3 = collection.Count - 1;
            collection.Add(endRight);
            idx4 = collection.Count - 1;

            AddTriangle(triangleIndices, idx3, idx2, idx1);
            AddTriangle(triangleIndices, idx2, idx3, idx4);
        }

        private GeometryModel3D CreateCylinder(double radius, double height, Material material, int sides)
        {
            Point3DCollection vertices = new Point3DCollection();

            double degreePerPoint = (2 * Math.PI) / sides;
            for (int i = 0; i < sides; i++)
            {
                Point3D point = new Point3D(radius * Math.Cos(i * degreePerPoint), 0.0f, radius * Math.Sin(i * degreePerPoint));
                vertices.Add(point);
                vertices.Add(new Point3D(point.X, height, point.Z));
            }

            Int32Collection triangleIndices = new Int32Collection();

            int nrVertices = vertices.Count;
            //Sides
            for(int i = 0; i < sides; i++)
            {
                AddTriangle(triangleIndices, (i * 2 + 1) % nrVertices, (i * 2 + 2) % nrVertices, (i * 2) % nrVertices);
                AddTriangle(triangleIndices, (i * 2 + 3) % nrVertices, (i * 2 + 2) % nrVertices, (i * 2 + 1) % nrVertices);
            }

            //Bottom
            var sequence = Enumerable.Range(0, sides).Select(n => n * 2).ToList();
            sequence.Reverse();
            AddCircle(triangleIndices, sequence);
            //Top
            sequence = Enumerable.Range(0, sides).Select(n => n * 2 + 1).ToList();
            AddCircle(triangleIndices, sequence);

            GeometryModel3D cylinderModel = new GeometryModel3D();
            MeshGeometry3D cylinderMesh = new MeshGeometry3D
            {
                Positions = vertices,
                TriangleIndices = triangleIndices
            };

            cylinderModel.Geometry = cylinderMesh;
            cylinderModel.Material = material;

            return cylinderModel;
        }

        private GeometryModel3D CreateCone(double radius, double height, Material material, int sides)
        {
            Point3DCollection vertices = new Point3DCollection();

            double degreePerPoint = (2 * Math.PI) / sides;
            for(int i = 0; i < sides; i++)
            {
                Point3D point = new Point3D(radius * Math.Cos(i * degreePerPoint), 0.0f, radius * Math.Sin(i * degreePerPoint));
                vertices.Add(point);
            }
            vertices.Add(new Point3D(0.0f, height, 0.0f));

            Int32Collection triangleIndices = new Int32Collection();

            //Sides

            int top = vertices.Count() - 1;
            for (int i = 0; i < sides; i++)
                AddTriangle(triangleIndices, top, (i + 1) % top, i % top);

            //Bottom
            var sequence = Enumerable.Range(0, sides - 1).ToList();
            sequence.Reverse();
            AddCircle(triangleIndices, sequence);

            GeometryModel3D coneModel = new GeometryModel3D();
            MeshGeometry3D coneMesh = new MeshGeometry3D
            {
                Positions = vertices,
                TriangleIndices = triangleIndices
            };

            coneModel.Geometry = coneMesh;
            coneModel.Material = material;

            return coneModel;
        }

        private GeometryModel3D CreateSurface(double[,] points, Material material)
        {
            Point3DCollection vertices = new Point3DCollection();

            int pointsX = points.GetLength(0);
            int pointsZ = points.GetLength(1);
            double xPerPoint = ChartStyle.SizeX / pointsX;
            double zPerPoint = ChartStyle.SizeZ / pointsZ;

            PointCollection textureCoords = new PointCollection();
            for (int i = 0; i < pointsX; i++)
                for (int j = 0; j < pointsZ; j++)
                {
                    vertices.Add(new Point3D(i * xPerPoint, points[i, j], j * zPerPoint * -1));
                    textureCoords.Add(new Point(0, 0));
                }

            Int32Collection triangleIndices = new Int32Collection();


            for (int i = 0; i < pointsX - 1; i++)
                for(int j = 0; j < pointsZ - 1; j++)
                {
                    int baseIndex = j * pointsX + i;

                    int idx1, idx2, idx3, idx4;

                    //Double vertices; for quad flat shading effect
                    vertices.Add(vertices[baseIndex]);
                    idx1 = vertices.Count - 1;
                    vertices.Add(vertices[baseIndex + 1]);
                    idx2 = vertices.Count - 1;
                    vertices.Add(vertices[baseIndex + pointsX]);
                    idx3 = vertices.Count - 1;
                    vertices.Add(vertices[baseIndex + pointsX + 1]);
                    idx4 = vertices.Count - 1;

                    AddTriangle(triangleIndices, idx3, idx2, idx1);
                    AddTriangle(triangleIndices, idx2, idx3, idx4);

                    var idx1Value = vertices[idx1].Y;
                    var idx2Value = vertices[idx2].Y;
                    var idx3Value = vertices[idx3].Y;
                    var idx4Value = vertices[idx4].Y;

                    double interval = (MaxY - MinY);
                    idx1Value /= interval;
                    idx2Value /= interval;
                    idx3Value /= interval;
                    idx4Value /= interval;
                    double average = idx1Value + idx2Value + idx3Value + idx4Value;
                    average /= 4;

                    textureCoords.Add(new Point(average, 0));
                    textureCoords.Add(new Point(average, 0));
                    textureCoords.Add(new Point(average, 0));
                    textureCoords.Add(new Point(average, 0));
                }




            GeometryModel3D surfaceModel = new GeometryModel3D();
            MeshGeometry3D surfaceMesh = new MeshGeometry3D
            {
                Positions = vertices,
                TriangleIndices = triangleIndices,
                TextureCoordinates = textureCoords
            };


            surfaceModel.Geometry = surfaceMesh;
            surfaceModel.Material = material;
            surfaceModel.BackMaterial = material;

            return surfaceModel;
        }

        private GeometryModel3D CreateWiremesh(double[,] points, Material material)
        {
            Point3DCollection vertices = new Point3DCollection();

            int pointsX = points.GetLength(0);
            int pointsZ = points.GetLength(1);
            double xPerPoint = ChartStyle.SizeX / pointsX;
            double zPerPoint = ChartStyle.SizeZ / pointsZ;

            for (int i = 0; i < pointsX; i++)
                for (int j = 0; j < pointsZ; j++)
                    vertices.Add(new Point3D(i * xPerPoint, points[i, j], j * zPerPoint * -1));

            Int32Collection triangleIndices = new Int32Collection();

            for (int i = 0; i < pointsX; i++)
                for(int j = 0; j < pointsZ; j++)
                {
                    int baseIndex = j * pointsX + i;

                    int rightNeighbour = baseIndex + 1;
                    int downNeighbour = baseIndex + pointsX;

                    if(rightNeighbour % pointsX > baseIndex % pointsX)
                        AddLine(vertices, triangleIndices, vertices[baseIndex], vertices[rightNeighbour], 0.03f);

                    if (downNeighbour < pointsX * pointsZ)
                        AddLine(vertices, triangleIndices, vertices[baseIndex], vertices[downNeighbour], 0.03f);
                }

            GeometryModel3D wiremeshModel = new GeometryModel3D();
            MeshGeometry3D wiremeshMesh = new MeshGeometry3D
            {
                Positions = vertices,
                TriangleIndices = triangleIndices
            };

            wiremeshModel.Geometry = wiremeshMesh;
            wiremeshModel.Material = material;
            wiremeshModel.BackMaterial = material;

            return wiremeshModel;
        }

        private DiffuseMaterial CreateDiffuseMaterial(Color color)
        {
            DiffuseMaterial material = new DiffuseMaterial();
            Brush brush = new SolidColorBrush(color);
            material.Brush = brush;
            return material;
        }

        #endregion

        #region Load

        private void LoadGraph()
        {


            Light.Children.Add(new AmbientLight());

            Camera.Position = new Point3D(ChartStyle.SizeX / 2.0f, ChartStyle.SizeY / 2.0f, -ChartStyle.SizeZ / 2.0f + CameraDistance);
            Camera.LookDirection = new Vector3D(0, 0, -1);
            
            Camera.FieldOfView = 60;

            GraphBorder.Child = Viewport;

            Viewport.Width = Width;
            Viewport.Height = Height;

            Viewport.IsHitTestVisible = false;
            

            ((INotifyCollectionChanged)Items).CollectionChanged += OnItemCollectionChanged;
            OnItemCollectionChanged(Items, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            AdjustViewSeries();
        }

        #endregion

        #region Layout

        private void UpdateTextblockLayout(TextBlock block, Color textColor, string binding, double fontSize, double rotation)
        {
            Binding textBinding = new Binding(binding)
            {
                Source = this
            };
            block.SetBinding(TextBlock.TextProperty, textBinding);
            block.FontSize = fontSize;
            block.Foreground = new SolidColorBrush(textColor);

            block.RenderTransform = new RotateTransform(rotation);
            block.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            block.Arrange(new Rect(block.DesiredSize));
        }



        private void UpdateTitleLayout()
        {
            if (!(Title is null))
            {
                UpdateTextblockLayout(TitleBlock, ChartStyle.TitleColor, "Title", ChartStyle.TitleFontSize, 0.0f);
                TitleBlock.Margin = new Thickness(CenterX - TitleBlock.ActualWidth / 2.0f, 0, 0, 0);
                MarginYTop += TitleBlock.ActualHeight;
            }

            CanvasElements.Add(TitleBlock);
        }

        private void UpdateGridLayout()
        {
            Model3DGroup grid = new Model3DGroup();

            var gridStyle = ChartStyle.GridStyle;

            if(!(gridStyle is null) && gridStyle.IsVisible)
            {
                

                var xInterval = gridStyle.IntervalX;
                var yInterval = gridStyle.IntervalY;
                var zInterval = gridStyle.IntervalZ;

                var xLength = ChartStyle.SizeX;
                var yLength = ChartStyle.SizeY;
                var zLength = ChartStyle.SizeZ;

                var xFactor = xLength / (MaxX - MinX);
                var yFactor = yLength / (MaxY - MinY);
                var zFactor = zLength / (MaxZ - MinZ);

                ScaleTransform3D scaleX = new ScaleTransform3D(1.0f, ChartStyle.SizeX, 1.0f, 0.0f, 0.0f, 0.0f);
                ScaleTransform3D scaleY = new ScaleTransform3D(1.0f, ChartStyle.SizeY, 1.0f, 0.0f, 0.0f, 0.0f);
                ScaleTransform3D scaleZ = new ScaleTransform3D(1.0f, ChartStyle.SizeZ, 1.0f, 0.0f, 0.0f, 0.0f);

                int xPositive = xFactor < 0 ? -1 : 1;
                int yPositive = yFactor < 0 ? -1 : 1;
                int zPositive = zFactor < 0 ? -1 : 1;

                if (gridStyle.IsXYVisible)
                {
                    var material = CreateDiffuseMaterial(gridStyle.ForegroundXY);
                    for(double pos = 0.0f; pos < Math.Abs(xLength); pos += Math.Abs(xInterval * xFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(0.0f, 0.0f, 1.0f), 0);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(pos * xPositive, 0.0f, 0.0f);

                        transform.Children.Add(scaleY);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }

                    for(double pos = 0.0f; pos < Math.Abs(yLength); pos += Math.Abs(yInterval * yFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(0.0f, 0.0f, 1.0f), 270);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(0.0f, pos * yPositive, 0.0f);

                        transform.Children.Add(scaleX);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }
                }

                if(gridStyle.IsXZVisible)
                {
                    var material = CreateDiffuseMaterial(gridStyle.ForegroundXZ);

                    for (double pos = 0.0f; pos < Math.Abs(xLength); pos += Math.Abs(xInterval * xFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(1.0f, 0.0f, 0.0f), 270);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(pos * xPositive, 0.0f, 0.0f);

                        transform.Children.Add(scaleZ);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }

                    for (double pos = 0.0f; pos < Math.Abs(zLength); pos += Math.Abs(zInterval * zFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(0.0f, 0.0f, 1.0f), 270);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(0.0f, 0.0f, -pos * zPositive);

                        transform.Children.Add(scaleX);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }
                }

                if(gridStyle.IsYZVisible)
                {
                    var material = CreateDiffuseMaterial(gridStyle.ForegroundYZ);

                    for (double pos = 0.0f; pos < Math.Abs(yLength); pos += Math.Abs(yInterval * yFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(1.0f, 0.0f, 0.0f), 270);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(0.0f, pos * yPositive, 0.0f);

                        transform.Children.Add(scaleZ);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }

                    for (double pos = 0.0f; pos < Math.Abs(zLength); pos += Math.Abs(zInterval * zFactor))
                    {
                        var line = CreateCylinder(gridStyle.Thickness, 1.0f, material, 8);
                        Transform3DGroup transform = new Transform3DGroup();

                        var rotate = new AxisAngleRotation3D(new Vector3D(0.0f, 0.0f, 1.0f), 0);
                        var rotation = new RotateTransform3D(rotate);
                        var translation = new TranslateTransform3D(0.0f, 0.0f, -pos * zPositive);

                        transform.Children.Add(scaleY);
                        transform.Children.Add(rotation);
                        transform.Children.Add(translation);
                        line.Transform = transform;

                        grid.Children.Add(line);
                    }
                }
            }
            Grid = grid;

            ViewportElements.Children.Add(Grid);
        }

        private void UpdateAxisLayout(Direction direction)
        {
            AxisStyle style = null;

            switch (direction)
            {
                case Direction.X:
                    style = ChartStyle.XAxisStyle;
                    break;
                case Direction.Y:
                    style = ChartStyle.YAxisStyle;
                    break;
                case Direction.Z:
                    style = ChartStyle.ZAxisStyle;
                    break;
                default:
                    break;
            }

            if(!(style is null) && style.IsVisible)
            {
                Model3DGroup axisGroup = new Model3DGroup();

                var material = CreateDiffuseMaterial(style.Color);
                var cylinder = CreateCylinder(style.Thickness, 1.0f, material, 16);
                axisGroup.Children.Add(cylinder);
                GeometryModel3D cone = null;

                if(style.DrawArrow)
                {
                    cone = CreateCone(style.Thickness * 2, style.Thickness, material, 16);
                    cone.Transform = new TranslateTransform3D(0.0f, 1.0f, 0.0f);
                    axisGroup.Children.Add(cone);
                }

                Transform3DGroup transformGroup = new Transform3DGroup();
                ScaleTransform3D scaleTransform = new ScaleTransform3D();
                RotateTransform3D rotationTransform = new RotateTransform3D();

                switch (direction)
                {
                    case Direction.X:
                        {
                            scaleTransform = new ScaleTransform3D(1.0f, ChartStyle.SizeX, 1.0f, 0.0f, 0.0f, 0.0f);
                            var axisAngleRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 270);
                            rotationTransform = new RotateTransform3D(axisAngleRotation, new Point3D());
                        }
                        break;
                    case Direction.Y:
                        {
                            scaleTransform = new ScaleTransform3D(1.0f, ChartStyle.SizeY, 1.0f, 0.0f, 0.0f, 0.0f);
                            var axisAngleRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
                            rotationTransform = new RotateTransform3D(axisAngleRotation, new Point3D());
                        }
                        break;
                    case Direction.Z:
                        {
                            scaleTransform = new ScaleTransform3D(1.0f, ChartStyle.SizeZ, 1.0f, 0.0f, 0.0f, 0.0f);
                            var axisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 270);
                            rotationTransform = new RotateTransform3D(axisAngleRotation, new Point3D());
                        }
                        break;
                    default:
                        break;
                }

                transformGroup.Children.Add(scaleTransform);
                transformGroup.Children.Add(rotationTransform);
                axisGroup.Transform = transformGroup;

                switch(direction)
                {
                    case Direction.X:
                        XAxisMesh = axisGroup;
                        break;
                    case Direction.Y:
                        YAxisMesh = axisGroup;
                        break;
                    case Direction.Z:
                        ZAxisMesh = axisGroup;
                        break;
                }
            }

        }

        private void UpdateAxisLayouts()
        {
            Markings.Children.Clear();

            UpdateAxisLayout(Direction.X);
            UpdateAxisLayout(Direction.Y);
            UpdateAxisLayout(Direction.Z);

            ViewportElements.Children.Add(XAxisMesh);
            ViewportElements.Children.Add(YAxisMesh);
            ViewportElements.Children.Add(ZAxisMesh);
        }



        private void UpdateContentLayout()
        {
            if (!(ChartStyle is null))
            {
                foreach (var series in ViewSeries)
                {

                    Model3DGroup content = new Model3DGroup();

                    DiffuseMaterial material = null;

                    switch(series.DrawStyle)
                    {
                        case SurfaceData.PresentationStyle.Area:
                            {
                                material = new DiffuseMaterial(series.Fill);
                                var geometry = CreateSurface(series.Points, material);
                                content.Children.Add(geometry);
                            }
                            break;
                        case SurfaceData.PresentationStyle.Lines:
                            {
                                material = new DiffuseMaterial(series.Outline);
                                var geometry = CreateWiremesh(series.Points, material);
                                content.Children.Add(geometry);
                            }
                            break;
                    }

                    ViewportElements.Children.Add(content);

                }
            }
        }

        private void UpdateAxisLabels()
        {
            Numbers.Clear();

            CanvasElements.Remove(XLabelBlock);
            CanvasElements.Remove(YLabelBlock);
            CanvasElements.Remove(ZLabelBlock);

            var xAxisHalfPoint = new Point3D(ChartStyle.SizeX / 2.0f, 0.0f, 0.0f);
            var yAxisHalfPoint = new Point3D(0.0f, ChartStyle.SizeY / 2.0f, 0.0f);
            var zAxisHalfPoint = new Point3D(0.0f, 0.0f, -(ChartStyle.SizeZ / 2.0f));

            var xAxisEndPoint = new Point3D(ChartStyle.SizeX, 0.0f, 0.0f);
            var yAxisEndPoint = new Point3D(0.0f, ChartStyle.SizeY, 0.0f);
            var zAxisEndPoint = new Point3D(0.0f, 0.0f, -ChartStyle.SizeZ);

            var screenPointX = Point3DToScreenCoords(Viewport, xAxisHalfPoint);
            var screenPointY = Point3DToScreenCoords(Viewport, yAxisHalfPoint);
            var screenPointZ = Point3DToScreenCoords(Viewport, zAxisHalfPoint);

            var screenPointEndX = Point3DToScreenCoords(Viewport, xAxisEndPoint);
            var screenPointEndY = Point3DToScreenCoords(Viewport, yAxisEndPoint);
            var screenPointEndZ = Point3DToScreenCoords(Viewport, zAxisEndPoint);

            var screenDirX = screenPointEndX - screenPointX;
            var screenDirY = screenPointEndY - screenPointY;
            var screenDirZ = screenPointEndZ - screenPointZ;

            var xStyle = ChartStyle.XAxisStyle;
            var yStyle = ChartStyle.YAxisStyle;
            var zStyle = ChartStyle.ZAxisStyle;

            Vector xDir = new Vector(1, 0);
            
            if (!(XLabel is null) && !(screenPointX is null) & !(screenPointEndX is null))
            {
                UpdateTextblockLayout(XLabelBlock, xStyle.Color, "XLabel", xStyle.FontSize, 0 * Vector.AngleBetween(xDir, screenDirX.Value));
                XLabelBlock.Margin = new Thickness(MarginXLeft + screenPointX.Value.X, MarginYTop + screenPointX.Value.Y, MarginXRight, MarginYBottom);
            }

            if (!(YLabel is null) && !(screenPointY is null) && !(screenPointEndY is null))
            {
                UpdateTextblockLayout(YLabelBlock, yStyle.Color, "YLabel", yStyle.FontSize, 0 * Vector.AngleBetween(xDir, screenDirY.Value));
                YLabelBlock.Margin = new Thickness(MarginXLeft + screenPointY.Value.X, MarginYTop + screenPointY.Value.Y, MarginXRight, MarginYBottom);
            }

            if(!(ZLabel is null) && !(screenPointZ is null) && !(screenPointEndZ is null))
            {
                UpdateTextblockLayout(ZLabelBlock, zStyle.Color, "ZLabel", zStyle.FontSize, 0 * Vector.AngleBetween(xDir, screenDirZ.Value));
                ZLabelBlock.Margin = new Thickness(MarginXLeft + screenPointZ.Value.X, MarginYTop + screenPointZ.Value.Y, MarginXRight, MarginYBottom);
            }

            CanvasElements.Add(XLabelBlock);
            CanvasElements.Add(YLabelBlock);
            CanvasElements.Add(ZLabelBlock);
        }


        private void UpdateViewportLayout(double graphSizeX, double graphSizeY)
        {

            Viewport.Children.Clear();
            ViewportElements.Children.Clear();

            Viewport.Width = graphSizeX;
            Viewport.Height = graphSizeY;

            Viewport.Camera = Camera;
            ViewportElements.Children.Add(Light);

            if (ChartStyle.DrawAxis) UpdateAxisLayouts();
            if (ChartStyle.DrawGrid) UpdateGridLayout();
            UpdateContentLayout();
            UpdateAxisLabels();

            ModelVisual3D graph = new ModelVisual3D
            {
                Content = ViewportElements
            };


            Viewport.Children.Add(graph);

            GraphBorder.Background = new SolidColorBrush(ChartStyle.BackgroundColor);

            GraphBorder.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, MarginXRight + ChartStyle.PaddingX, MarginYBottom + ChartStyle.PaddingY);
        }

        private void UpdateGraphLayout()
        {
            if(IsVisible && IsLoaded)
            {
                Items.Remove(GraphBorder);
                foreach (var element in CanvasElements)
                    Items.Remove(element);

                CanvasElements.Clear();

                MarginYTop = 0;
                MarginYBottom = 0;
                MarginXLeft = 0;
                MarginXRight = 0;
                CenterX = ActualWidth / 2.0f;
                CenterY = ActualHeight / 2.0f;

                if (MinX >= MaxX || MinY >= MaxY || MinZ >= MaxZ)
                    throw new ArgumentException();

                UpdateTitleLayout();

                AdjustViewSeries();

                double graphSizeX = ActualWidth - MarginXLeft - MarginXRight - 2 * ChartStyle.PaddingX;
                double graphSizeY = ActualHeight - MarginYTop - MarginYBottom - 2 * ChartStyle.PaddingY;

                if (graphSizeX > 0 && graphSizeY > 0)
                    UpdateViewportLayout(graphSizeX, graphSizeY);

                Items.Add(GraphBorder);
                foreach (var element in CanvasElements)
                    Items.Add(element);

            }
        }

        #endregion

        #region Events

        private void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Series.Clear();
            foreach (var item in Items)
            {
                var series = item as SurfaceData;
                if (!(series is null))
                    Series.Add(series);
            }
            AdjustViewSeries();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadGraph();
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateGraphLayout();
            base.OnRender(drawingContext);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            var position = e.GetPosition(Viewport);

            if (position.X > 0 && position.Y > 0 && position.X < Viewport.ActualWidth && position.Y < Viewport.ActualHeight)
            {
                Rotate = true;
                MoveStart = position;
            }
            
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(Rotate)
            {
                var diff = e.GetPosition(Viewport) - MoveStart;
                var rotY = diff.X / 100.0f;
                var rotUp = diff.Y / 100.0f;
                CameraRotY += rotY;
                CameraRotY %= 2 * Math.PI;

                CameraRotUp += rotUp;
                CameraRotUp %= 2 * Math.PI;
                if (CameraRotUp > Math.PI / 3)
                    CameraRotUp = Math.PI / 3;
                else if (CameraRotUp < -(Math.PI / 3))
                    CameraRotUp = -(Math.PI / 3);

                var sin = Math.Sin(CameraRotY);
                var cos = Math.Cos(CameraRotY);

                Camera.LookDirection = new Vector3D(-cos, 0.0f, -sin);
                Camera.Position = new Point3D(ChartStyle.SizeX / 2.0f + cos * CameraDistance, ChartStyle.SizeY / 2.0f, -ChartStyle.SizeZ / 2.0f + sin * CameraDistance);

                var rotation = new AxisAngleRotation3D(new Vector3D(-sin, 0.0f, cos), (CameraRotUp / (2 * Math.PI)) * 360);
                Camera.Transform = new RotateTransform3D(rotation, new Point3D(ChartStyle.SizeX / 2.0f, ChartStyle.SizeY / 2.0f, -ChartStyle.SizeZ / 2.0f));
                
                MoveStart = e.GetPosition(Viewport);

                UpdateAxisLabels();
            }
            base.OnMouseMove(e);
        }

        

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Rotate = false;
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var position = e.GetPosition(Viewport);

            if (position.X > 0 && position.Y > 0 && position.X < Viewport.ActualWidth && position.Y < Viewport.ActualHeight)
            {
                CameraDistance -= e.Delta / 120.0f;
                var pos = Camera.Position;
                var vec = new Vector3D(pos.X - ChartStyle.SizeX / 2.0f, pos.Y - ChartStyle.SizeY / 2.0f, pos.Z + ChartStyle.SizeZ / 2.0f);
                vec.Normalize();
                pos = new Point3D(ChartStyle.SizeX / 2.0f + vec.X * CameraDistance, ChartStyle.SizeY / 2.0f + vec.Y * CameraDistance, -ChartStyle.SizeZ / 2.0f + vec.Z * CameraDistance);
                Camera.Position = pos;

                UpdateAxisLabels();
            }
            base.OnMouseWheel(e);
        }

        #endregion


    }
}
