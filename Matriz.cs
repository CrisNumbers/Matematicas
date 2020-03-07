using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matematicas
{
    public class Matriz
    {
        public class MatrizException : Exception
        {
            public static readonly string SizeError = "El tamaño de filas y columnas de ambas matrices no coinciden";
            public MatrizException(string message) : base(message)
            {
                message = "MatrizException: " + message;
            }
        }


        public readonly int cantFila;
        public readonly int cantColumna;

        float[,] matrix;

        public static void Copiar(Matriz origen, ref Matriz destino)
        {
            if (origen.cantColumna != destino.cantColumna || origen.cantFila != destino.cantFila)
                throw new MatrizException(MatrizException.SizeError);

            for (int fil = 0; fil < origen.cantFila; fil++)
                for (int col = 0; col < origen.cantColumna; col++)
                    origen.matrix[fil, col] = destino.matrix[fil, col];
        }

        public static void Copiar(float[,] origen, ref Matriz destino)
        {
            if (origen.GetLength(0) != destino.cantColumna || origen.GetLength(1) != destino.cantFila)
                throw new MatrizException(MatrizException.SizeError);

            for (int fil = 0; fil < destino.cantFila; fil++)
                for (int col = 0; col < destino.cantColumna; col++)
                    origen[fil, col] = destino.matrix[fil, col];
        }

        #region Constructor
        public Matriz(int filaP, int columnaP)
        {
            cantFila = filaP;
            cantColumna = columnaP;

            matrix = new float[cantFila, cantColumna];
        }

        public Matriz(float[,] matrizP)
        {
            cantFila = matrizP.GetLength(0);
            cantColumna = matrizP.GetLength(1);

            matrix = new float[cantFila, cantColumna];

            for (int fil = 0; fil < this.cantFila; fil++)
                for (int col = 0; col < this.cantColumna; col++)
                    this.matrix[fil, col] = matrizP[fil, col];
        }
        #endregion

        #region Matriz_Retornos
        public static Matriz Identidad2x2
        {
            get
            {
                return new Matriz(new float[2, 2] { { 1, 0 }, { 0, 1 } });
            }
        }
        public static Matriz Identidad3x3
        {
            get
            {
                return new Matriz(new float[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });
            }
        }
        public static Matriz Identidad4x4
        {
            get
            {
                return new Matriz(new float[4, 4] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
            }
        }
        public static Matriz Nula2x2
        {
            get
            {
                return new Matriz(2, 2);
            }
        }
        public static Matriz Nula3x3
        {
            get
            {
                return new Matriz(3, 3);
            }
        }
        public static Matriz Nula4x4
        {
            get
            {
                return new Matriz(4, 4);
            }
        }
        public static Matriz Punto(float x, float y, float z)
        {
            return new Matriz(new float[4, 1] { { x }, { y }, { z }, { 1 } });
        }
        public static Matriz Traslacion(float x, float y, float z)
        {
            Matriz retorno = Matriz.Identidad4x4;
            retorno.matrix[3, 0] = x;
            retorno.matrix[3, 1] = y;
            retorno.matrix[3, 2] = z;
            return retorno;
        }
        public static Matriz Escala(float x, float y, float z)
        {
            Matriz retorno = Matriz.Nula4x4;
            retorno.matrix[0, 0] = x;
            retorno.matrix[1, 1] = y;
            retorno.matrix[2, 2] = z;
            return retorno;
        }
        #endregion

        #region Matriz_Matematica
        public static Matriz Suma(Matriz m1, Matriz m2)
        {
            if (m1.cantColumna != m2.cantColumna || m1.cantFila != m2.cantFila)
                throw new MatrizException(MatrizException.SizeError);

            Matriz matrizResultado = new Matriz(m1.cantFila, m1.cantColumna);

            for (int fil = 0; fil < m1.cantFila; fil++)
                for (int col = 0; col < m1.cantColumna; col++)
                    matrizResultado.matrix[fil, col] = m1.matrix[fil, col] + m2.matrix[fil, col];
            return matrizResultado;
        }
        public static Matriz Resta(Matriz m1, Matriz m2)
        {
            if (m1.cantColumna != m2.cantColumna || m1.cantFila != m2.cantFila)
                throw new MatrizException(MatrizException.SizeError);

            Matriz matrizResultado = new Matriz(m1.cantFila, m1.cantColumna);

            for (int fil = 0; fil < m1.cantFila; fil++)
                for (int col = 0; col < m1.cantColumna; col++)
                    matrizResultado.matrix[fil, col] = m1.matrix[fil, col] - m2.matrix[fil, col];
            return matrizResultado;
        }
        public static Matriz Multiplicacion(Matriz m, float escalar)
        {
            Matriz matrizResultado = new Matriz(m.cantFila, m.cantColumna);
            for (int fil = 0; fil < m.cantFila; fil++)
                for (int col = 0; col < m.cantColumna; col++)
                    matrizResultado.matrix[fil, col] = m.matrix[fil, col] * escalar;
            return matrizResultado;
        }
        public static Matriz Multiplicacion(Matriz m1, Matriz m2)
        {
            if (m1.cantColumna != m2.cantFila)
                throw new MatrizException(MatrizException.SizeError);

            Matriz matrizResultado = new Matriz(m1.cantFila, m2.cantColumna);
            float auxValor;
            for (int filM1 = 0; filM1 < m1.cantFila; filM1++)
            {
                for (int colM2 = 0; colM2 < m2.cantColumna; colM2++)
                {
                    auxValor = 0;
                    for (int mix = 0; mix < m2.cantFila; mix++)
                    {
                        auxValor += m1.matrix[filM1, mix] * m2.matrix[mix, colM2];
                    }
                    matrizResultado.matrix[filM1, colM2] = auxValor;
                }
            }
            return matrizResultado;
        }
        public Matriz Opuesta()
        {
            Matriz matrizResultado = new Matriz(cantFila, cantColumna);
            for (int fil = 0; fil < cantFila; fil++)
                for (int col = 0; col < cantColumna; col++)
                    matrizResultado.matrix[fil, col] = -matrix[fil, col];
            return matrizResultado;
        }
        public Matriz Transpuesta()
        {
            Matriz matrizResultado = new Matriz(cantColumna, cantFila);
            for (int fil = 0; fil < cantFila; fil++)
                for (int col = 0; col < cantColumna; col++)
                    matrizResultado.matrix[col, fil] = matrix[fil, col];
            return matrizResultado;
        }
        public static bool EsIgual(Matriz m1, Matriz m2)
        {
            bool isEquals = true;
            try
            {
                if (m1.cantColumna != m2.cantColumna || m1.cantFila != m2.cantFila)
                    throw new MatrizException(MatrizException.SizeError);

                for (int fil = 0; fil < m1.cantFila && isEquals; fil++)
                    for (int col = 0; col < m1.cantColumna && isEquals; col++)
                        if (m1.matrix[fil, col] != m2.matrix[fil, col])
                            isEquals = false;
            }
            catch (Exception)
            {
                isEquals = false;
            }
            return isEquals;
        }
        public static bool EsOpuesta(Matriz m1, Matriz m2)
        {
            return EsIgual(m1, m2.Opuesta());
        }
        public static bool EsTranspuesta(Matriz m1, Matriz m2)
        {
            return EsIgual(m1, m2.Transpuesta());
        }
        public static bool EsCuadrada(Matriz m1)
        {
            return m1.cantColumna == m1.cantFila;
        }
        public static bool EsRectangulo(Matriz m1)
        {
            return m1.cantColumna != m1.cantFila;
        }
        #endregion

        #region SobreCargas
        public override bool Equals(object obj)
        {
            try
            {
                Matriz objeto = obj as Matriz;
                return EsIgual(this, objeto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public override string ToString()
        {
            string impresion = "\n";

            for (int fil = 0; fil < this.cantFila; fil++)
            {
                impresion += "[ ";
                for (int col = 0; col < this.cantColumna; col++)
                {
                    impresion += this.matrix[fil, col].ToString();
                    if (col != this.cantColumna - 1)
                        impresion += " , ";
                }
                impresion += " ]\n";
            }


            return impresion;
        }
        #endregion

        #region Operadores
        public static Matriz operator +(Matriz m1, Matriz m2)
        {
            return Suma(m1, m2);
        }
        public static Matriz operator -(Matriz m1, Matriz m2)
        {
            return Resta(m1, m2);
        }
        public static Matriz operator *(Matriz m, float escalar)
        {
            return Multiplicacion(m, escalar);
        }
        public static Matriz operator *(Matriz m1, Matriz m2)
        {
            return Multiplicacion(m1, m2);
        }
        public static bool operator ==(Matriz m1, Matriz m2)
        {
            return EsIgual(m1, m2);
        }
        public static bool operator !=(Matriz m1, Matriz m2)
        {
            return !EsIgual(m1, m2);
        }
        public float this[int i, int j]
        {
            get { return this.matrix[i, j]; }
            set { this.matrix[i,j] = value; }
        }
        #endregion
    }
}
