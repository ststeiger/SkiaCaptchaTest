
namespace SkiaCaptcha
{


    internal class MathHelpers 
    {

        private static System.Random seed = new System.Random();

        public static double rand(int min, int max)
        {
            return seed.Next(min, max + 1);
        } // End Function rand 


        public static double[] addVector(double[] a, double[] b)
        {
            return new double[] { a[0] + b[0], a[1] + b[1], a[2] + b[2] };
        } // End Function addVector 


        public static double[] scalarProduct(double[] vector, double scalar)
        {
            return new double[] { vector[0] * scalar, vector[1] * scalar, vector[2] * scalar };
        } // End Function scalarProduct 


        public static double dotProduct(double[] a, double[] b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        } // End Function dotProduct 


        public static double norm(double[] vector)
        {
            return System.Math.Sqrt(dotProduct(vector, vector));
        } // End Function norm 


        public static double[] normalize(double[] vector)
        {
            return scalarProduct(vector, 1 / norm(vector));
        } // End Function normalize 


        // http://en.wikipedia.org/wiki/Cross_product
        public static double[] crossProduct(double[] a, double[] b)
        {
            return new double[]{
		        (a[1] * b[2] - a[2] * b[1]),
		        (a[2] * b[0] - a[0] * b[2]),
		        (a[0] * b[1] - a[1] * b[0])
		    };
        } // End Function crossProduct 


        public static double[] vectorProductIndexed(double[] v, double[] m, int i)
        {
            return new double[]{
		        v[i + 0] * m[0] + v[i + 1] * m[4] + v[i + 2] * m[8] + v[i + 3] * m[12],
		        v[i + 0] * m[1] + v[i + 1] * m[5] + v[i + 2] * m[9] + v[i + 3] * m[13],
		        v[i + 0] * m[2] + v[i + 1] * m[6] + v[i + 2] * m[10]+ v[i + 3] * m[14],
		        v[i + 0] * m[3] + v[i + 1] * m[7] + v[i + 2] * m[11]+ v[i + 3] * m[15]
            };
        } // End Function vectorProductIndexed 


        public static double[] vectorProduct(double[] v, double[] m)
        {
            return vectorProductIndexed(v, m, 0);
        } // End Function vectorProduct 


        public static double[] matrixProduct(double[] a, double[] b)
        {
            double[] o1 = vectorProductIndexed(a, b, 0);
            double[] o2 = vectorProductIndexed(a, b, 4);
            double[] o3 = vectorProductIndexed(a, b, 8);
            double[] o4 = vectorProductIndexed(a, b, 12);

            return new double[]{
		        o1[0], o1[1], o1[2], o1[3],
		        o2[0], o2[1], o2[2], o2[3],
		        o3[0], o3[1], o3[2], o3[3],
		        o4[0], o4[1], o4[2], o4[3]
            };
        } // End Function matrixProduct 


        // http://graphics.idav.ucdavis.edu/education/GraphicsNotes/Camera-Transform/Camera-Transform.html
        public static double[] cameraTransform(double[] C, double[] A)
        {
            double[] w = normalize(addVector(C, scalarProduct(A, -1)));
            double[] y = new double[] { 0, 1, 0 };
            double[] u = normalize(crossProduct(y, w));
            double[] v = crossProduct(w, u);
            double[] t = scalarProduct(C, -1);

            return new double[]{
		        u[0], v[0], w[0], 0,
		        u[1], v[1], w[1], 0,
		        u[2], v[2], w[2], 0,
		        dotProduct(u, t), dotProduct(v, t), dotProduct(w, t), 1
            };
        } // End Function cameraTransform 


        // http://graphics.idav.ucdavis.edu/education/GraphicsNotes/Viewing-Transformation/Viewing-Transformation.html
        public static double[] viewingTransform(double fov, double n, double f)
        {
            fov *= (System.Math.PI / 180.0);
            double cot = 1 / System.Math.Tan(fov / 2);

            return new double[]{
		        cot,	0,		0,		0,
		        0,		cot,	0,		0, 
		        0,		0,		(f + n) / (f - n),		-1,
		        0,		0,		2 * f * n / (f - n),	0
            };
        } // End Function viewingTransform 


    } // End Class Helpers 


} // End Namespace GdTest 
