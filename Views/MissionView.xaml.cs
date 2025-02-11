using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Trace.Common.Events;
using Trace.Common.Extensions;
using Trace.Gmap;
using static Trace.Gmap.AMapProviderBase;

namespace Trace.Views
{
    /// <summary>
    /// MissionView.xaml 的交互逻辑
    /// </summary>
    public partial class MissionView : UserControl
    {
        private readonly IEventAggregator eventAggregator;
        List<PointLatLng> points = new List<PointLatLng>();
        public MissionView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            this.Map_Loaded();//加载地图

        }

        private void Map_Loaded()
        {
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("ditu.google.cn");
            }
            catch
            {
                MainMap.Manager.Mode = AccessMode.CacheOnly;
                System.Windows.MessageBox.Show("没有可用的internet连接，正在进入缓存模式!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            MainMap.CacheLocation = Environment.CurrentDirectory + "\\GMapCache\\"; //缓存位置
            MainMap.MapProvider = AMapProvider.Instance; //加载高德地图
            MainMap.MinZoom = 2;  //最小缩放
            MainMap.MaxZoom = 17; //最大缩放
            MainMap.Zoom = 8;     //当前缩放
            MainMap.ShowCenter = false; //不显示中心十字点
            MainMap.DragButton = MouseButton.Left; //右键拖拽地图
            MainMap.Position = new PointLatLng(39.909149, 116.397486); //地图中心位置：北京
            //MainMap.MouseLeftButtonDown += new MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
            MainMap.MouseRightButtonDown += new MouseButtonEventHandler(MainMap_MouseRightButtonDown);

          /*  List<PointLatLng> points = new List<PointLatLng>();
            //在这块添加一些points

            points.Add(new PointLatLng(39.906217, 116.3912757));
            points.Add(new PointLatLng(30.2489634, 120.2052342));
            points.Add(new PointLatLng(35.2489634, 120.2052342));
            GMapRoute route = new GMapRoute(points);//
            route.ZIndex = 10;
            route.RegenerateShape(MainMap);
            (route.Shape as Path).Stroke = Brushes.Blue;
            (route.Shape as Path).StrokeThickness = 5;
            (route.Shape as Path).Effect = null;


            MainMap.Markers.Add(route);*/

        
            eventAggregator.ReceiveCoordinates(ShowRoute);
            eventAggregator.ReceiveNewCoordinate(AddRoute);
            
        }

        private void AddRoute(CoordinatesInTrip trip)
        {
            foreach (var coord in trip.Coordinates)
            {
                points.Add(new PointLatLng((double)coord.Latitude, (double)coord.Longitude)); // 将你的坐标转换为PointLatLng格式
            }
            GMapRoute route = new GMapRoute(points);
            // 设置路线的一些属性，比如颜色、宽度等（可选）
            route.Shape = new Path() { StrokeThickness = 1, Stroke = Brushes.Red };

            // 获取地图控件实例（这里假设你能通过某种方式获取到，比如依赖注入或者在视图代码中传递引用等）
            MainMap.Markers.Add(route);
        }

        private void ShowRoute(CoordinatesInTrip trip)
        {
            MainMap.Markers.Clear();
            points.Clear();
            foreach (var coord in trip.Coordinates)
            {
                points.Add(new PointLatLng((double)coord.Latitude, (double)coord.Longitude)); // 将你的坐标转换为PointLatLng格式
            }

          /* var currentMarker = new GMapMarker(MainMap.Position);
            {
                currentMarker.Shape = new CustomMarker(this, currentMarker, "custom position marker");
                currentMarker.Offset = new System.Windows.Point(-15, -15);
                currentMarker.ZIndex = int.MaxValue;
                MainMap.Markers.Add(currentMarker);
            }*/

            GMapRoute route = new GMapRoute(points);
            // 设置路线的一些属性，比如颜色、宽度等（可选）
            route.Shape = new Path() { StrokeThickness = 1, Stroke = Brushes.Red };

            // 获取地图控件实例（这里假设你能通过某种方式获取到，比如依赖注入或者在视图代码中传递引用等）
            MainMap.Markers.Add(route);
        }



        /// <summary>
        /// 右键划线
        /// </summary>
        int id = 1;

        PointLatLng point_last;
        void MainMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

          //  MainMap.Markers.Add(new GMapMarker(new PointLatLng()));


            System.Windows.Point clickPoint = e.GetPosition(MainMap);
            PointLatLng point_show = MainMap.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);



            var existingMarker = MainMap.Markers.FirstOrDefault(m => m.Tag != null && m.Tag.ToString() == "1");
            // 检查是否已经存在该货车的标记，如果存在则更新位置，否则添加新的标记
            if (existingMarker!=null)
            {
                // 更新标记位置
                existingMarker.Position = new PointLatLng(point_show.Lat, point_show.Lng);
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(typeof(Geometry));
                GMapMarker marker = new GMapMarker(new PointLatLng(point_show.Lat, point_show.Lng));
                marker.Tag = "1"; // 使用货车ID作为标记的标签，以便识别

                Path markerShape = new Path()
                {
                    StrokeThickness = 1,
                    Stretch = Stretch.Fill,

                    Stroke = Brushes.Red
                    ,
                    Width = 20,
                    Height = 20,
                    Data = (Geometry)converter.ConvertFrom("M 808.6 403.2 c 0 -178.8 -129.8 -308.5 -308.5 -308.5 c -170.1 0 -308.5 138.4 -308.5 308.5 c 0 125.6 170.6 338.3 262.3 452.6 l 6.8 8.4 c 9.6 12 24 18.9 39.5 18.9 c 15.4 0 29.8 -6.9 39.5 -18.9 l 6.8 -8.4 c 91.5 -114.3 262.1 -327 262.1 -452.6 Z m -310.1 89.4 c -62.9 0 -114 -51.1 -114 -114 s 51.1 -114 114 -114 s 114 51.1 114 114 s -51.1 114 -114 114 Z M 500.1 67.8 c -184.9 0 -335.4 150.4 -335.4 335.4 c 0 135 174.5 352.5 268.2 469.4 l 6.7 8.4 c 14.8 18.4 36.8 29 60.4 29 s 45.6 -10.6 60.4 -29 l 6.8 -8.4 C 661 755.7 835.4 538.2 835.4 403.2 c 0 -194.3 -141 -335.4 -335.3 -335.4 Z m 0 815.3 c -15.4 0 -29.8 -6.9 -39.5 -18.9 l -6.8 -8.4 c -91.7 -114.3 -262.3 -327 -262.3 -452.6 c 0 -170.1 138.4 -308.5 308.5 -308.5 c 178.8 0 308.5 129.8 308.5 308.5 c 0 125.6 -170.6 338.3 -262.3 452.6 l -6.8 8.4 c -9.5 12 -23.9 18.9 -39.3 18.9 Z")
                };
                //(currentMarker.Shape as CustomMarker).SetContent(point, "1"); 这种方法可以触发SetContent
                marker.Shape = markerShape;
                marker.Offset = new System.Windows.Point(-markerShape.Width / 2, -markerShape.Height);
                marker.ZIndex = -1;
                marker.Position = point_show;
                
                // 添加新的标记
                MainMap.Markers.Add(marker);
            }


            if (id > 1)
            {
                GMapRoute gmRoute = new GMapRoute(new List<PointLatLng>() {

point_last, //上一次的位置

point_show //当前显示的位置

});

                gmRoute.Shape = new Path() { StrokeThickness = 1, Stroke = Brushes.Red };



                MainMap.Markers.Add(gmRoute);
            }
            id += 1;
            point_last = point_show;


        }


        private void OnlocationReceived(GeoLocation location)
        {
            // 创建或更新标记
            GMapMarker marker = new GMapMarker(new PointLatLng(location.Latitude, location.Latitude));
            marker.Tag = location.TruckID; // 使用货车ID作为标记的标签，以便识别

            // 检查是否已经存在该货车的标记，如果存在则更新位置，否则添加新的标记
            if (MainMap.Markers.Any(m => m.Tag.ToString() == location.TruckID.ToString()))
            {
                // 更新标记位置
                var existingMarker = MainMap.Markers.FirstOrDefault(m => m.Tag.ToString() == location.TruckID.ToString());
                existingMarker.Position = new PointLatLng(location.Latitude, location.Latitude);
            }
            else
            {
                // 添加新的标记
                MainMap.Markers.Add(marker);
            }

            // 根据需要，您可以在这里添加逻辑来保存路径点，以便后续绘制路径

            //在这块添加一些points


            GMapRoute route = (GMapRoute)MainMap.Markers.FirstOrDefault(r => r.Tag.ToString() == location.TruckID.ToString());
            //GMapRoute route = new GMapRoute(points);//
            route.ZIndex = 10;
            route.RegenerateShape(MainMap);
            (route.Shape as Path).Stroke = Brushes.Blue;
            (route.Shape as Path).StrokeThickness = 5;
            (route.Shape as Path).Effect = null;


            MainMap.Markers.Add(route);
        }

        
    }
}
