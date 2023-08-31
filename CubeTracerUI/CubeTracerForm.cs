using CubeTools;
using CubeTracer.CubeTools;
using CubeTracer.DataAccess.Models;
using CubeTypes;


namespace CubeTracerUI
{
    public partial class CubeTracerForm : Form
    {
        //holds current scramble
        string scramble;
        //holds current cube
        Cube newCube;
        //holds current reconstruction
        ReconstructionModel recon;
        //Lists of information to display on form
        List<ImageModel> edgeImages;
        List<ImageModel> cornerImages;
        List<AlgModel> edgeAlgs;
        List<AlgModel> cornerAlgs;

        /// <summary>
        /// Constructor
        /// 1. Makes new cube
        /// 2. Populates scramble colors
        /// 3. Generate memos
        /// 4. Reconstructs cube
        /// </summary>
        public CubeTracerForm()
        {

            InitializeComponent();
            GenerateNewCube();
            GenerateColors();
            GenerateMemos();
            ReconstructCube();
         


        }
       
        /// <summary>
        /// Method to update all lists portrayed on the form.
        /// This method is called every time a new cube is reconstructed
        /// </summary>
        private void WireUpLists()
        {
            EdgeImagesListBox.DataSource = null;
            EdgeImagesListBox.DataSource = edgeImages;
            EdgeImagesListBox.DisplayMember = "DisplayName";
            EdgeAlgsListBox.DataSource = null;
            EdgeAlgsListBox.DataSource = edgeAlgs;
            EdgeAlgsListBox.DisplayMember = "DisplayName";
            CornerImagesListBox.DataSource = null;
            CornerImagesListBox.DataSource = cornerImages;
            CornerImagesListBox.DisplayMember = "DisplayName";
            CornerAlgsListBox.DataSource = null;
            CornerAlgsListBox.DataSource = cornerAlgs;
            CornerAlgsListBox.DisplayMember = "DisplayName";
        }
        /// <summary>
        /// Method to recontstruct a new cube
        /// Updates all the list fields with
        /// current cube information
        /// </summary>
        private void ReconstructCube()
        {
            recon = Reconstructor.Reconstruct(newCube.EdgeMemo, newCube.CornerMemo);
            edgeImages = recon.EdgeImages;
            cornerImages = recon.CornerImages;
            edgeAlgs = recon.EdgeAlgs;
            cornerAlgs = recon.CornerAlgs;
            WireUpLists();
        }
        /// <summary>
        /// Method to call CubeTracer and trace the current cube.
        /// This information is needed before the cube can be reconstructed
        /// and information can be displayed.
        /// </summary>
        private void GenerateMemos()
        {
            CubeTools.CubeTracer.traceCube(newCube);
            EdgeMemoTextBox.Text = newCube.EdgeMemo;
            CornerMemoTextBox.Text = newCube.CornerMemo;
            Reconstructor.Reconstruct(newCube.EdgeMemo, newCube.CornerMemo);
        }
        /// <summary>
        /// This method is responsible for the correct colors being
        /// portrayed in the scramble image.
        /// </summary>
        private void GenerateColors()
        {
            UBLColorBox.BackColor = GetColor(newCube.ColorString[0]);
            UBColorBox.BackColor = GetColor(newCube.ColorString[1]);
            UBRColorBox.BackColor = GetColor(newCube.ColorString[2]);
            ULColorBox.BackColor = GetColor(newCube.ColorString[3]);
            UCenterColorBox.BackColor = GetColor(newCube.ColorString[4]);
            URColorBox.BackColor = GetColor(newCube.ColorString[5]);
            UFLColorBox.BackColor = GetColor(newCube.ColorString[6]);
            UFColorBox.BackColor = GetColor(newCube.ColorString[7]);
            UFRColorBox.BackColor = GetColor(newCube.ColorString[8]);
            RUFColorBox.BackColor = GetColor(newCube.ColorString[9]);
            RUColorBox.BackColor = GetColor(newCube.ColorString[10]);
            RUBColorBox.BackColor = GetColor(newCube.ColorString[11]);
            RFColorBox.BackColor = GetColor(newCube.ColorString[12]);
            RCenterColorBox.BackColor = GetColor(newCube.ColorString[13]);
            RBColorBox.BackColor = GetColor(newCube.ColorString[14]);
            RDFColorBox.BackColor = GetColor(newCube.ColorString[15]);
            RDColorBox.BackColor = GetColor(newCube.ColorString[16]);
            RDBColorBox.BackColor = GetColor(newCube.ColorString[17]);
            FULColorBox.BackColor = GetColor(newCube.ColorString[18]);
            FUColorBox.BackColor = GetColor(newCube.ColorString[19]);
            FURColorBox.BackColor = GetColor(newCube.ColorString[20]);
            FLColorBox.BackColor = GetColor(newCube.ColorString[21]);
            FCenterColorBox.BackColor = GetColor(newCube.ColorString[22]);
            FRColorBox.BackColor = GetColor(newCube.ColorString[23]);
            FDLColorBox.BackColor = GetColor(newCube.ColorString[24]);
            FDColorBox.BackColor = GetColor(newCube.ColorString[25]);
            FDRColorBox.BackColor = GetColor(newCube.ColorString[26]);
            DFLColorBox.BackColor = GetColor(newCube.ColorString[27]);
            DFColorBox.BackColor = GetColor(newCube.ColorString[28]);
            DFRColorBox.BackColor = GetColor(newCube.ColorString[29]);
            DLColorBox.BackColor = GetColor(newCube.ColorString[30]);
            DCenterColorBox.BackColor = GetColor(newCube.ColorString[31]);
            DRColorBox.BackColor = GetColor(newCube.ColorString[32]);
            DBLColorBox.BackColor = GetColor(newCube.ColorString[33]);
            DBColorBox.BackColor = GetColor(newCube.ColorString[34]);
            DBRColorBox.BackColor = GetColor(newCube.ColorString[35]);
            LUBColorBox.BackColor = GetColor(newCube.ColorString[36]);
            LUColorBox.BackColor = GetColor(newCube.ColorString[37]);
            LUFColorBox.BackColor = GetColor(newCube.ColorString[38]);
            LBColorBox.BackColor = GetColor(newCube.ColorString[39]);
            LCenterColorBox.BackColor = GetColor(newCube.ColorString[40]);
            LFColorBox.BackColor = GetColor(newCube.ColorString[41]);
            LDBColorBox.BackColor = GetColor(newCube.ColorString[42]);
            LDColorBox.BackColor = GetColor(newCube.ColorString[43]);
            LDFColorBox.BackColor = GetColor(newCube.ColorString[44]);
            BURColorBox.BackColor = GetColor(newCube.ColorString[45]);
            BUColorBox.BackColor = GetColor(newCube.ColorString[46]);
            BULColorBox.BackColor = GetColor(newCube.ColorString[47]);
            BRColorBox.BackColor = GetColor(newCube.ColorString[48]);
            BCenterColorBox.BackColor = GetColor(newCube.ColorString[49]);
            BLColorBox.BackColor = GetColor(newCube.ColorString[50]);
            BDRColorBox.BackColor = GetColor(newCube.ColorString[51]);
            BDColorBox.BackColor = GetColor(newCube.ColorString[52]);
            BDLColorBox.BackColor = GetColor(newCube.ColorString[53]);
            }
        /// <summary>
        /// returns correct color based on given char c
        /// </summary>
        /// <param name="c">always either U, F, R, B, L or D</param>
        /// <returns></returns>
        private Color GetColor(char c)
        {
            if (c == 'U')
            {
                return Color.White;
            }
            else if (c == 'F')
            {
                return Color.Lime;
            }
            else if (c == 'R')
            {
                return Color.Red;
            }
            else if (c == 'B')
            {
                return Color.Blue;
            }
            else if (c == 'L')
            {
                return Color.Orange;
            }
            else
            {
                return Color.Yellow;
            }
        }
        /// <summary>
        /// Method that handles a new scramble being requested
        /// Follows the same exact instructions as the constructor method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void NewScrambleButton_Click(object sender, EventArgs e)
        {
            GenerateNewCube();
            GenerateColors();
            GenerateMemos();
            ReconstructCube();

        }
        private void GenerateNewCube()
        {

            newCube = new Cube();
            scramble = Scrambler.scrambleCube(newCube);
            ScrambleValueTextBox.Text = scramble;

        }
       //EVERYTHING BELOW THIS POINT IS USELESS BUT I'M SCARED TO TOUCH IT

        private void pictureBox47_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox49_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void CubeTracerForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {

        }

        private void UCenterColorBox_Click(object sender, EventArgs e)
        {

        }

        private void UBLColorBox_Click(object sender, EventArgs e)
        {

        }

        private void EdgeMemoTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CornerImagesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EdgeAlgorithmsLabel_Click(object sender, EventArgs e)
        {

        }
    }
}