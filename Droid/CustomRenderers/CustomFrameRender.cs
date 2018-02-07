using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Graphics;
using Steamboat.Mobile.CustomControls;
using Steamboat.Mobile.Droid.CustomRenderers;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace Steamboat.Mobile.Droid.CustomRenderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        Context _localContext;

        public CustomFrameRenderer(Context context) : base(context)
        {
            _localContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Invalidate(); //Force draw
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CustomFrame.IsActive))
            {
                Invalidate();
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            DrawOutline(canvas, canvas.Width, canvas.Height);
        }
        void DrawOutline(Canvas canvas, int width, int height)
        {
            var element = Element as CustomFrame;

            using (var paint = new Paint { AntiAlias = true })
            using (var path = new Path())
            using (Path.Direction direction = Path.Direction.Cw)
            using (Paint.Style style = Paint.Style.Stroke)
            using (var rect = new RectF(0, 0, width, height))
            {
                float rx = _localContext.ToPixels(element.CornerRadius);
                float ry = _localContext.ToPixels(element.CornerRadius);
                path.AddRoundRect(rect, rx, ry, direction);

                Android.Graphics.Color borderColor = element.IsActive?
                                                     element.ActiveOutlineColor.ToAndroid():element.OutlineColor.ToAndroid();
                paint.StrokeWidth = (float)(element.IsActive ? element.ActiveBorderWidth : element.DefaultBorderWidth);
                paint.SetStyle(style);
                paint.Color = borderColor;
                canvas.DrawPath(path, paint);
            }
        }
    }
}
