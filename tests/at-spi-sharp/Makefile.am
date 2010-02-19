
EXTRA_DIST =  

ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -keyfile:$(top_srcdir)/mono-uia.snk -noconfig -codepage:utf8 -warn:4 -warnaserror -optimize+ -debug "-define:DEBUG"

ASSEMBLY = bin/AtSpiSharpTests.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
BUILD_DIR = bin

ATSPISHARP_DLL_SOURCE=../../at-spi/bin/at-spi-sharp.dll
ATSPISHARPTESTS_DLL_MDB_SOURCE=bin/at-spiTests.dll.mdb
ATSPISHARPTESTS_DLL_MDB=$(BUILD_DIR)/at-spiTests.dll.mdb
ATSPISHARP_DLL_CONFIG=$(BUILD_DIR)/at-spi-sharp.dll.config
ATSPISHARP_DLL_CONFIG_SOURCE=../../at-spi/bin/at-spi-sharp.dll.config
ATSPISHARP_DLL_MDB_SOURCE=../../at-spi/bin/at-spi-sharp.dll.mdb
ATSPISHARP_DLL_MDB=$(BUILD_DIR)/at-spi-sharp.dll.mdb


#AL=al2
#SATELLITE_ASSEMBLY_NAME=MonoTests.Mono.UIAutomation.Winforms.resources.dll

PROGRAMFILES = \
	$(ATSPISHARP_DLL) \
	$(ATSPITESTS_DLL_MDB) \
	$(ATSPISHARP_DLL_CONFIG)  
	$(ATSPISHARP_DLL_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES)

FILES =  \
	AssemblyInfo.cs \
	AccessibleTest.cs \
	ActionTest.cs \
	Base.cs \
	ComponentTest.cs \
	DeviceEventControllerTest.cs \
	DocumentTest.cs \
	EventTest.cs \
	HypertextTest.cs \
	ImageTest.cs \
	SelectionTest.cs \
	TableTest.cs \
	TextTest.cs \
	UncachedChildrenTest.cs

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	System \
	System.Data \
	Mono.Posix \
	-pkg:mono-nunit \
	../../at-spi/bin/at-spi-sharp.dll

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) \
	bin/TestResult.xml

include $(top_srcdir)/Makefile.include

ATSPISHARP_DLL = $(BUILD_DIR)/at-spi-sharp.dll

$(eval $(call emit-deploy-target,ATSPISHARP_DLL))
$(eval $(call emit-deploy-target,ATSPISHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,ATSPISHARP_DLL_MDB))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	@mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	@chmod +x atspitest.sh
