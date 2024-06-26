using System.Collections.Generic;
using System;

using Skyline.DataMiner.Analytics.GenericInterface;
using System.Globalization;

namespace GQIIntegrationSPI
{
    [GQIMetaData(Name = "fake event data")]
    public class GQIDataSourceFromCSVDouble : IGQIDataSource, IGQIInputArguments
    {
        private readonly GQIDateTimeArgument _argStart = new GQIDateTimeArgument("Start Time") { IsRequired = true };
        private readonly GQIDateTimeArgument _argEnd = new GQIDateTimeArgument("End Time") { IsRequired = true };
        private readonly GQIIntArgument _argIntervals = new GQIIntArgument("Number of intervals") { IsRequired = true };

        private DateTime _Start;
        private DateTime _End;
        private int _NumberOfIntervals;

        public GQIArgument[] GetInputArguments()
        {
            return new GQIArgument[] { _argStart, _argEnd, _argIntervals };
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            _Start = args.GetArgumentValue(_argStart);
            _End = args.GetArgumentValue(_argEnd);
            _NumberOfIntervals = args.GetArgumentValue(_argIntervals);

            return new OnArgumentsProcessedOutputArgs();
        }

        /// <inheritdoc />
        public GQIColumn[] GetColumns()
        {
            return new GQIColumn[]
            {
                new GQIStringColumn("Type"),
                new GQIStringColumn("Value"),
                new GQIDateTimeColumn("Start"),
                new GQIDateTimeColumn("End")
            };
        }

        /// <inheritdoc />
        public GQIPage GetNextPage(GetNextPageInputArgs args)
        {
            List<GQIRow> rows = new List<GQIRow>();
            TimeSpan interval = TimeSpan.FromTicks((_End - _Start).Ticks / _NumberOfIntervals);
            DateTime start = _Start;
            DateTime end = start + interval;

            for (int i = 0; i < _NumberOfIntervals; i++)
            {


                rows.Add(new GQIRow(new[] {
                    new GQICell { Value = "Pattern Info" },
                    new GQICell { Value = GetPatternInfo() },
                    new GQICell { Value = start.ToUniversalTime()},
                    new GQICell { Value = end.ToUniversalTime()}
                    }));
                start = end;
                end = start + interval;

                rows.Add(new GQIRow(new[] {
                    new GQICell { Value = "User Action" },
                    new GQICell { Value = GetUserAction() },
                    new GQICell { Value = start.ToUniversalTime()},
                    new GQICell { Value = end.ToUniversalTime()}
                    }));

                rows.Add(new GQIRow(new[] {
                    new GQICell { Value = "Weather" },
                    new GQICell { Value = GetWeather() },
                    new GQICell { Value = start.ToUniversalTime()},
                    new GQICell { Value = end.ToUniversalTime()}
                    }));

                rows.Add(new GQIRow(new[] {
                    new GQICell { Value = "Booking Info" },
                    new GQICell { Value = GetBookingInfo() },
                    new GQICell { Value = start.ToUniversalTime()},
                    new GQICell { Value = end.ToUniversalTime()}
                    }));

                rows.Add(new GQIRow(new[] {
                    new GQICell { Value = "Anomaly Info" },
                    new GQICell { Value = GetAnomalyInfo() },
                    new GQICell { Value = start.ToUniversalTime()},
                    new GQICell { Value = end.ToUniversalTime()}
                    }));

            }


            return new GQIPage(rows.ToArray())
            {
                HasNextPage = false
            };
        }

        private Random _Random = new Random();
        private String GetWeather()
        {
            string[] options = { "rain", "sun" };

            // Generate a random index
            int index = _Random.Next(0, options.Length);

            // Select the string based on the index
            return options[index];
        }

        private String GetAnomalyInfo()
        {
            string[] options = { "none", "none", "none", "none", "none", "none", "none", "none", "Level Shift", "Flatline" };

            // Generate a random index
            int index = _Random.Next(0, options.Length);

            // Select the string based on the index
            return options[index];
        }

        private String GetBookingInfo()
        {
            string[] options = { "none", "none", "none", "none", "none", "none", "none", "none", "On Air", "none" };

            // Generate a random index
            int index = _Random.Next(0, options.Length);

            // Select the string based on the index
            return options[index];
        }

        private String GetUserAction()
        {
            string[] options = { "none", "none", "none", "none", "none", "none", "none", "none", "Leaving", "Entering" };

            // Generate a random index
            int index = _Random.Next(0, options.Length);

            // Select the string based on the index
            return options[index];
        }

        private String GetPatternInfo()
        {
            string[] options = { "none", "none", "none", "none", "none", "none", "Backup", "none", "None", "None" };

            // Generate a random index
            int index = _Random.Next(0, options.Length);

            // Select the string based on the index
            return options[index];
        }
    }
}