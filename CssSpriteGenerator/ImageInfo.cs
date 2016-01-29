using Mapper;
using System.Drawing;
using System;
using System.IO;

namespace CssSpriteGenerator
{
  internal class ImageInfo : IImageInfo
  {
    private ImageInfo( Image image, string name )
    {

      Image = image;
      Name = name;


    }


    public Image Image
    {
      get; private set;
    }

    public string Name
    {
      get; private set;
    }


    public static ImageInfo Create( string path )
    {

      try
      {

        var image = Image.FromFile( path );
        return new ImageInfo( image, Path.GetFileName( path ).Replace( ".", "-" ) );

      }
      catch
      {
        return null;
      }



    }


    public int Height
    {
      get
      {
        return Image.Height;
      }
    }

    public int Width
    {
      get
      {
        return Image.Width;
      }
    }
  }
}