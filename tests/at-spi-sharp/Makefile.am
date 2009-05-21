
EXTRA_DIST =  

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -keyfile:../../mono-uia.snk -noconfig -codepage:utf8 -warn:4 -warnaserror -optimize+ -debug "-define:DEBUG"

ASSEMBLY = bin/Debug/at-spi-sharp-tests.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../at-spi/bin/Debug/at-spi-sharp.dll
BUILD_DIR = bin/Debug

ATSPISHARP_DLL_SOURCE=../../at-spi/bin/Debug/at-spi-sharp.dll
ATSPISHARPTESTS_DLL_MDB_SOURCE=bin/Debug/at-spiTests.dll.mdb
ATSPISHARPTESTS_DLL_MDB=$(BUILD_DIR)/at-spiTests.dll.mdb
ATSPISHARP_DLL_MDB_SOURCE=../../at-spi/bin/Debug/at-spi-sharp.dll.mdb
ATSPISHARP_DLL_MDB=$(BUILD_DIR)/at-spi-sharp.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -keyfile:../../mono-uia.snk -noconfig -codepage:utf8 -warn:4 -warnaserror -optimize+
ASSEMBLY = bin/Release/at-spiSharpTests.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../at-spi/bin/Release/at-spi-sharp.dll

BUILD_DIR = bin/Release

ATSPISHARP_DLL_SOURCE=../../at-spi/bin/Release/at-spi-sharp.dll
ATSPISHARPTESTS_DLL_MDB=
ATSPISHARP_DLL_MDB=

endif

#AL=al2
#SATELLITE_ASSEMBLY_NAME=MonoTests.Mono.UIAutomation.Winforms.resources.dll

PROGRAMFILES = \
	$(ATSPISHARP_DLL) \
	$(ATSPITESTS_DLL_MDB) \
	$(ATSPISHARP_DLL_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES)

FILES =  \
	AssemblyInfo.cs \
	ActionTest.cs \
	Base.cs \
	ComponentTest.cs \
	ImageTest.cs \
	SelectionTest.cs \
	TextTest.cs

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	System \
	System.Data \
	Mono.Posix \
	-pkg:mono-nunit \
	../../at-spi/bin/Debug/at-spi-sharp.dll \
	$(NDESK_DBUS_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) \
	bin/Debug/TestResult.xml

include $(top_srcdir)/Makefile.include

ATSPISHARP_DLL = $(BUILD_DIR)/at-spi-sharp.dll

$(eval $(call emit-deploy-target,ATSPISHARP_DLL))
$(eval $(call emit-deploy-target,ATSPISHARP_DLL_MDB))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	@mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	@chmod +x atspitest.sh
