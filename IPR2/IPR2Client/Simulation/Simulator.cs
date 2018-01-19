﻿using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Simulator : Form
    {
        private readonly Measurement Measurement;
        private int _time;
        private string _name;

        public Simulator(string name)
        {
            _name = name;
            _time = 0;
            SimpleTime temp = new SimpleTime(_time / 60, _time % 60);
            Measurement = new Measurement(20, 100, 50, temp.Minutes, temp.Seconds);
            InitializeComponent();
            FormClosing += Simulator_FormClosing;

            weerstand.Text = Measurement.Weerstand + "";
            hartslag.Text = Measurement.Hartslag + "";
            rondes.Text = Measurement.Rondes + "";
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void weerstandMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand > 4)
                Measurement.Weerstand -= 5;
            weerstand.Text = Measurement.Weerstand + "";
        }

        private void weerstandPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand < 96)
                Measurement.Weerstand += 5;
            weerstand.Text = Measurement.Weerstand + "";
        }

        private void hartslagMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag > 4)
                Measurement.Hartslag -= 5;
            hartslag.Text = Measurement.Hartslag + "";
        }

        private void hartslagPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag < 196)
                Measurement.Hartslag += 5;
            hartslag.Text = Measurement.Hartslag + "";
        }

        private void rondesMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes > 4)
                Measurement.Rondes -= 5;
            rondes.Text = Measurement.Rondes + "";
        }

        private void rondesPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes < 196)
                Measurement.Rondes += 5;
            rondes.Text = Measurement.Rondes + "";
        }

        public string ReadLine() {
            Measurement measurement = UpdateSim();

            return measurement.ToRawData();
        }

        public void Write(string command) {
            // TODO: Handle commands (is this even usefull?)
        }

        public Measurement UpdateSim()
        {
            if (_time < 5999)
            {
                _time++;
                Measurement.Time = new SimpleTime(_time / 60, _time % 60);
            }
            tijd.Text = Measurement.Time.ToString();

            Measurement measurement = new Measurement(
                Measurement.Weerstand,
                Measurement.Hartslag,
                Measurement.Rondes,
                Measurement.Time
            );

            return measurement;
        }
    }
}
