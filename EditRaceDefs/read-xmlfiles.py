import glob

path = '../1.3/Defs/'
file_path_lists = glob.glob("{}/**/*.xml".format(path), recursive=True)
print(file_path_lists)