using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UtilityPack.Function {

    public class SixSigma <T> where T : new() {

        List<double> collections = null; //list of value
        int n = 0; //size of subgroups
        double Center = 0.0; // value center
        double LCL = 0.0; //lower limit control 
        double UCL = 0.0; //upper limit control

        double x_tb = 0; //process average
        double s = 0; //sigma
        double s3 = 0; // three sigma

        public bool isvalidcollection = false; //flag check list of value valid or not (True = valid, False = not valid)


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="name_of_sigma_variable"></param>
        /// <param name="value_lcl"></param>
        /// <param name="value_center"></param>
        /// <param name="value_ucl"></param>
        public SixSigma(List<T> ts, string name_of_sigma_variable, double value_lcl, double value_center, double value_ucl) {

            //get bound -------------------//
            this.Center = value_center;
            this.LCL = value_lcl;
            this.UCL = value_ucl;

            //get collection value --------//
            collections = new List<double>();
            //---
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int _index = 0;
            for (int i = 0; i < properties.Length; i++) {
                if (properties[i].Name.Equals(name_of_sigma_variable) == true) {
                    _index = i;
                    break;
                }
            }

            //---
            double min = double.MaxValue;
            foreach (var t in ts) {
                double x;
                bool r = double.TryParse(string.Format("{0}", properties[_index].GetValue(t, null)), out x);
                double v = r == true ? x : double.MinValue;
                if (v < min) min = v;
                collections.Add(v);
            }
            //check list of value valid or not
            isvalidcollection = min == double.MinValue ? false : true;

            //get size of subgroups -----//
            this.n = collections.Count;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="value_lcl"></param>
        /// <param name="value_center"></param>
        /// <param name="value_ucl"></param>
        public SixSigma(List<double> ts, double value_lcl, double value_center, double value_ucl) {

            //get bound -------------------//
            this.Center = value_center;
            this.LCL = value_lcl;
            this.UCL = value_ucl;

            //get collection value --------//
            collections = new List<double>();
            collections = ts;

            //get size of subgroups -----//
            this.n = collections.Count;
        }

        /// <summary>
        /// Tính giá trị trung bình của tập lấy mẫu, X_tb = (tổng giá trị tập hợp / số lượng lấy mẫu)
        /// </summary>
        /// <returns></returns>
        public double getProcessAverage() {
            x_tb = Math.Round(collections.Sum() / n, 7);
            return x_tb;
        }

        /// <summary>
        /// Tính khoảng biến thiên R, R = Giá trị Max - Giá trị Min
        /// </summary>
        /// <returns></returns>
        public double getRange() {
            return Math.Round(collections.Max() - collections.Min(), 7);
        }

        /// <summary>
        /// Tính giá trị phương sai, S2
        /// </summary>
        /// <returns></returns>
        public double getVariance() {
            double sum = 0.0;
            double process_average = x_tb == 0 ?  this.getProcessAverage() : x_tb;
            
            foreach (var i in collections) {
                double s = Math.Pow(i - process_average, 2.0);
                sum += s;
            }

            return Math.Round(sum / (n - 1), 7);
        }

        /// <summary>
        /// Tính giá trị 1 sigma, S
        /// </summary>
        /// <returns></returns>
        public double getSigmaValue() {
            s = Math.Round(Math.Sqrt(this.getVariance()), 7);
            return s;
        }

        /// <summary>
        /// Tính giá trị 2 sigma, 3 sigma, 4 sigma...
        /// </summary>
        /// <param name="multiplier_value"></param>
        /// <returns></returns>
        public double getMultiplierSigmaValue(int multiplier_value) {
            double _sigma = s == 0 ? getSigmaValue() : s;
            return multiplier_value * _sigma;
        }

        /// <summary>
        /// Tính giá trị 3 sigma
        /// </summary>
        /// <returns></returns>
        public double getThreeSigmaValue() {
            double _sigma = s == 0 ? getSigmaValue() : s;
            return 3 * _sigma;
        }

        /// <summary>
        /// Tính giá trị 6 sigma
        /// </summary>
        /// <returns></returns>
        public double getSixSigmaValue() {
            double _sigma = s == 0 ? getSigmaValue() : s;
            return 6 * _sigma;
        }

        /// <summary>
        /// Tính giá trị sai số chuẩn, SE
        /// </summary>
        /// <returns></returns>
        public double getSigmaError() {
            double _sigma = s == 0 ? getSigmaValue() : s;
            return Math.Round(_sigma / Math.Sqrt(n), 7);
        }

        /// <summary>
        /// Tính giá trị Delta(x) = Center - ProcessAverage
        /// </summary>
        /// <returns></returns>
        public double getDeltax() {
            double process_average = x_tb == 0 ? this.getProcessAverage() : x_tb;
            return Math.Round(Center - process_average, 7);
        }


        /// <summary>
        /// Tính giá trị Cpu
        /// </summary>
        /// <returns></returns>
        public double getCpu() {
            double process_average = x_tb == 0 ? this.getProcessAverage() : x_tb;
            s3 = s3 == 0 ? this.getMultiplierSigmaValue(3) : s3;
            return Math.Round((UCL - process_average)/ s3, 7);
        }

        /// <summary>
        /// Tính giá trị Cpl
        /// </summary>
        /// <returns></returns>
        public double getCpl() {
            double process_average = x_tb == 0 ? this.getProcessAverage() : x_tb;
            s3 = s3 == 0 ? this.getMultiplierSigmaValue(3) : s3;
            return Math.Round((process_average - LCL) / s3, 7);
        }

        /// <summary>
        /// Tính giá trị Cpk
        /// </summary>
        /// <returns></returns>
        public double getCpk() {
            return Math.Min(this.getCpu(), this.getCpl());
        }

    }
}
