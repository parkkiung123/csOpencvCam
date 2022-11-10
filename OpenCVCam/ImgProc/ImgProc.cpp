#include "pch.h"

#include "ImgProc.h"

bool ImgProc::Func::Init(System::String^ filename)
{
	if (movie != NULL)
		return false;
	movie = new OpenCVApp::Movie();
	init = movie->Init(msclr::interop::marshal_as<std::string>(filename));
	return init;
}

bool ImgProc::Func::GetFrame(cli::array<unsigned char>^ dst)
{
	if (!init)
		return false;
	pin_ptr<unsigned char> dp = &dst[0];
	return movie->GetFrame(dp);
}

void ImgProc::Func::Terminate()
{
	if (movie != NULL)
	{
		movie->Terminate();
		delete movie;
		movie = NULL;
	}
}

int ImgProc::Func::GetWidth()
{
	if (!init)
		return false;
	return movie->GetWidth();
}

int ImgProc::Func::GetHeight()
{
	if (!init)
		return false;
	return movie->GetHeight();
}

double ImgProc::Func::GetFPS()
{
	if (!init)
		return false;
	return movie->GetFPS();
}