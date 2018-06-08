using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamicRendering
{
    class Program
    {
        static void Main(string[] args)
        {
            // create instance of ContentType

            var ct = new ContentType()
            {
                Images = new List<Image>()
                {
                    new Image()
                    {
                        AltText="prod1",
                        Url="https://www.istartedsomething.com/bingimages/#20180605-us"
                    },
                    new Image()
                    {
                        AltText="prod2",
                        Url="https://www.istartedsomething.com/bingimages/?m=5&y=2018#20180519-us"
                    }
                },
                Tag = "products"
            };

            // get typename and serialize content to Json

            var tn = ct.GetType().AssemblyQualifiedName;

            // create Json representation of instance

            var jsonObj = JsonConvert.SerializeObject(ct);

            // pretend to be on 'the other/client side' and you just have the typename (tn) and the json representation of the class

            Console.WriteLine("Received on the client side via REST call");
            Console.WriteLine($"The typename string received: {tn}");
            Console.WriteLine($"The json of the instance: {jsonObj}");

            // create an instance of the right type of json and tn

            var jsonConvertResult = JsonConvert.DeserializeObject(jsonObj, Type.GetType(tn));

            ObjRender(jsonConvertResult);

            Console.ReadLine();
        }

        private static void ObjRender(object objectToRender)
        {
            Console.WriteLine($"{Environment.NewLine}-- begin render {objectToRender.GetType().Name}");
            foreach (var prop in objectToRender.GetType().GetProperties())
            {
                Console.WriteLine(PropRender(prop, objectToRender));
            }
            Console.WriteLine($"-- end render {objectToRender.GetType().Name}{Environment.NewLine}");

        }

        private static string PropRender(PropertyInfo propertyinfo, object parent)
        {
            if (propertyinfo.PropertyType.GetNestedTypes().Length > 0)
            {
                //if (propertyinfo.PropertyType.IsAssignableFrom(Type.GetType("System.Collections.Generic.List")))
                //{
                    var nestedObj = (List<dynamic>)propertyinfo.GetMethod.Invoke(parent, null);
                    foreach (var item in nestedObj)
                    {
                        ObjRender(item);
                    }
                //}
                //var nestedObj = propertyinfo.GetMethod.Invoke(parent,null);
            }

            return $"{propertyinfo.Name} = {propertyinfo.GetValue(parent)} (type:{propertyinfo.PropertyType.FullName})";
        }

    }



    public class ContentType
    {
        public string Tag { get; set; }
        public List<Image> Images { get; set; }
    }
    public class Image
    {
        public string Url { get; set; }
        public string AltText { get; set; }

    }
}
