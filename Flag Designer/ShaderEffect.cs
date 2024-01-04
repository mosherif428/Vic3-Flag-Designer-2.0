using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;

namespace Flag_Designer
{
    public class OverlayShaderEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(OverlayShaderEffect), 0);

        public static readonly DependencyProperty OverlayColorProperty =
            DependencyProperty.Register("OverlayColor", typeof(Color), typeof(OverlayShaderEffect), new UIPropertyMetadata(Colors.Red, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty BlendModeProperty =
            DependencyProperty.Register("BlendMode", typeof(float), typeof(OverlayShaderEffect), new UIPropertyMetadata(0.5f, PixelShaderConstantCallback(1)));

        public OverlayShaderEffect()
        {
            PixelShader = new PixelShader();
            PixelShader.UriSource = new Uri("OverlayShader.hlsl", UriKind.Relative);
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(OverlayColorProperty);
            UpdateShaderValue(BlendModeProperty);
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public Color OverlayColor
        {
            get { return (Color)GetValue(OverlayColorProperty); }
            set { SetValue(OverlayColorProperty, value); }
        }

        public float BlendMode
        {
            get { return (float)GetValue(BlendModeProperty); }
            set { SetValue(BlendModeProperty, value); }
        }
    }
}
