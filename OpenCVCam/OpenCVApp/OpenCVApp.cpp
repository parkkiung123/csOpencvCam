#include "OpenCVApp.h"

bool OpenCVApp::Movie::Init(string filename)
{
	cap = VideoCapture(filename);
	init = cap.isOpened();
	if (init)
	{
		width = (int)cap.get(CAP_PROP_FRAME_WIDTH);
		height = (int)cap.get(CAP_PROP_FRAME_HEIGHT);
		fps = (double)cap.get(CAP_PROP_FPS);
	}
	return init;
}

bool OpenCVApp::Movie::GetFrame(unsigned char* dp)
{
	if (!init)
		return false;
	frame = Mat(height, width, CV_8UC3, (void*)dp);
	return cap.read(frame);
}

void OpenCVApp::Movie::Terminate()
{
	if (!init)
		return;
	else
		cap.release();
}

int OpenCVApp::Movie::GetWidth()
{
	if (!init)
		return 0;
	return width;
}

int OpenCVApp::Movie::GetHeight()
{
	if (!init)
		return 0;
	return height;
}

double OpenCVApp::Movie::GetFPS()
{
	if (!init)
		return 0;
	return fps;
}
