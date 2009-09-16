EXTRA_DIST =
	gtkbutton.py \
	gtkhscale.py \
	gtktextview.py \
	gtktreeview.py \
	opensuse60x38.gif

ASSEMBLY = DocumentTest.exe
BUILD_DIR = .

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG"

ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 

DOCUMENTTEST_EXE_MDB_SOURCE=DocumentTest.exe.mdb
DOCUMENTTEST_EXE_MDB=$(BUILD_DIR)/DocumentTest.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -keyfile:../mono-uia.snk
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 

DOCUMENTTEST_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(DOCUMENTTEST_EXE_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES)

FILES =  \
	Document.cs

DATA_FILES = 

EXTRAS = 

REFERENCES =  \
	System \
	Mono.Posix \
	System.Core \
	$(GTK_SHARP_20_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES)

include $(top_srcdir)/Makefile.include


$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
