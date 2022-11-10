#pragma once
#include <string>
#include <opencv2/core.hpp>
#include <opencv2/videoio.hpp>
using namespace cv;
using namespace std;

namespace OpenCVApp
{
	class Movie
	{
	private:
		VideoCapture cap;
		Mat frame;
		bool init = false;
		int width;
		int height;
		double fps;
	public:
		bool Init(string filename);
		bool GetFrame(unsigned char* dp);
		void Terminate();

		int GetWidth();
		int GetHeight();
		double GetFPS();
	};
}


