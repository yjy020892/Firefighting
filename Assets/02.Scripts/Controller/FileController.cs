using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileController
{
    private static FileController _instance = null;
    public static FileController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new FileController();
            return _instance;
        }
    }

    public DirectoryInfo[] findFoldersByPath(string url)
    {
        //폴더안의 폴더정보 찾아주는 함수.
        //List<string> folderNameList = new List<string>();
        DirectoryInfo directoryInfo = new DirectoryInfo(url);
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        return directoryInfos;
    }

    public DirectoryInfo findFloderByPath(string url)
    {
         DirectoryInfo directoryInfo = new DirectoryInfo(url);
          return directoryInfo;
    }

    public FileInfo[] findfilesByPath(string floderPath)
    {
        //폴더안의 파일 경로들 찾아주는 함수.
        List<string> fileNameList = new List<string>();
        DirectoryInfo directoryInfo = new DirectoryInfo(floderPath);
        FileInfo[] fileinfos = directoryInfo.GetFiles();
        return fileinfos;
    }

    public FileInfo[] checkFileExtension(FileInfo[] pathList, List<string> exclusionList)
    {
        //path 리스트에서 특정 확장자 골라내고 다시 반환해주는 함수.
        List<FileInfo> newPathList = new List<FileInfo>();
        for (int a = 0; a < pathList.Length; a++)
        {
            bool isState = false;
            string[] spltes = pathList[a].ToString().Split('.');
            for (int b = 0; b < spltes.Length; b++)
            {
                for (int c = 0; c < exclusionList.Count; c++)
                {
                    if (spltes[b] == exclusionList[c].ToString().ToLower())
                    {
                        isState = true;
                    }
                }
            }
            if (!isState)
                newPathList.Add(pathList[a]);
        }
        FileInfo[] newFileInfos = new FileInfo[newPathList.Count];
        for (int a = 0; a < newFileInfos.Length; a++)
            newFileInfos[a] = newPathList[a];
        return newFileInfos;
    }
}
