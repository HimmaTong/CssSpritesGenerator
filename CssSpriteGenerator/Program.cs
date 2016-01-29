using Mapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssSpriteGenerator
{
  class Program
  {
    static void Main( string[] args )
    {

      var images = EnumerateImages( Environment.CurrentDirectory );

      Console.WriteLine( Environment.CurrentDirectory );

      foreach ( var item in images )
      {
        Console.WriteLine( item.Name );
      }

      CreateSprite( images );


    }

    private static void CreateSprite( ImageInfo[] images )
    {
      var mapper = new MapperOptimalEfficiency<Sprite>( new Canvas() );
      var sprite = mapper.Mapping( images );
      var spriteImage = DrawSprite( sprite );

      var imagePath = Path.Combine( Environment.CurrentDirectory, "sprites.png" );
      spriteImage.Save( imagePath, ImageFormat.Png );


      var styles = GenerateStyleSheet( sprite, imagePath );
      File.WriteAllText( Path.Combine( Environment.CurrentDirectory, "sprites.css" ), styles );
    }

    private static Image DrawSprite( Sprite sprite )
    {

      var canvas = new Bitmap( sprite.Width, sprite.Height );
      var graphic = Graphics.FromImage( canvas );

      foreach ( var item in sprite.MappedImages )
      {
        graphic.DrawImage( ((ImageInfo) item.ImageInfo).Image, item.X, item.Y );
      }


      return canvas;
    }


    private static string GenerateStyleSheet( Sprite sprite, string spriteImage )
    {

      var writer = new StringWriter();

      foreach ( var item in sprite.MappedImages )
      {
        writer.WriteLine( "." + ((ImageInfo) item.ImageInfo).Name );
        writer.WriteLine( "{" );
        writer.WriteLine( "  background-image: url('{0}');", Path.GetFileName( spriteImage ) );
        writer.WriteLine( "  background-position: {0}px {1}px;", -item.X, -item.Y );
        writer.WriteLine( "  width: {0}px;", item.ImageInfo.Width );
        writer.WriteLine( "  height: {0}px;", item.ImageInfo.Height );
        writer.WriteLine( "}" );
      }

      return writer.ToString();

    }

    private static ImageInfo[] EnumerateImages( string path )
    {

      return Directory.EnumerateFiles( path, "*.png" )
        .Select( filepath => ImageInfo.Create( filepath ) )
        .Where( item => item != null ).ToArray();

    }
  }
}
