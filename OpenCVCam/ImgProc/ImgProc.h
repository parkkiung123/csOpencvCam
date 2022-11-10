#pragma once
#pragma managed
#include <msclr\marshal_cppstd.h>
#pragma unmanaged
#include "../OpenCVApp/OpenCVApp.h"
#pragma managed

namespace ImgProc {
	public ref class Func
	{
	private:
		OpenCVApp::Movie* movie = NULL;
		bool init = false;
	public:
		~Func()
		{
			this->!Func();
		}
		!Func()
		{
			Terminate();
		}
		bool Init(System::String^ filename);
		bool GetFrame(cli::array<unsigned char>^ dst);
		void Terminate();

		int GetWidth();
		int GetHeight();
		double GetFPS();
	};
}
