### Table of Contents
> [Summary](#summary)<br>
> [Development Guide](#development-guide-br)<br>
>     > [Implementation Instructions](#implementation-instructions)<br>
>     > [PR Template](#pr-template)<br>
>     > [Coding Conventions](#coding-conventions)<br>
> [Resource Materials](#resource-materials)<br>
> [Authors](#authors)<br>

<br><br>

# <INSERT_NAME> by Ebb & Flow Studios


## Summary
Capstone project for the IT:IMD program at Carleton University. Includes an interactive VR experience where the user journeys through the life of a box turtle, navigating the dangers brought upon by climate change.<br><br>


## Development Guide 
<br>

### Implementation Instructions
When contributing to the development of the project, please follow the instructions below:
1. **Select Task** - Navigate to the notion development kanban board and select the task at the top of the list (organized in order of importance)<br><br>
2. **Mine Requirements** - Research, collect, and document necessary materials from online sources and/or other group members related to the task (e.g., UX design document + objectives, environment credentials, etc.)<br><br>
3. **Development Plan** - Create a draft development plan, which includes a software design document with pseudocode implementations and testing procedures *(requires at least one approval from the UX design team and/or dev team lead)*<br><br>
4. **Core Development**

    4.1. **TODO Comments** - Add `TODO` comments based on software design document to plan implementation 

    4.2. **Implement Code** - Program core implementations, ensuring to test and commit each stage after successful implementation

    4.3. **Unit & Integration Tests** - If applicable, add automated unit & integration tests to verify the output of functions based on test cases<br><br>

5. **Post PR** - Push all commits and post a PR, following the PR template below<br><br>
6. **Merge PR** - After 2 approvals and successfully solved all merge conflicts, merge the code into the main branch<br><br>
7. **Document Task** - Update the status of the task in Notion and add any related documentation for future implementations *(including any external references)*<br><br>

    7.1. **Notify Members** - Post a message into the Discord channel 'dev' about the new implementation<br><br>

### PR Template
Please follow the template below when posting a Pull Request (PR) to apply changes on the main branch: 

> ### Title
> 
> **Corresponding Task:** *(link to notion task and/or task name & id)*
> 
> **Overview:** *(1-2 sentences detailing what is being implemented and/or changed)*
> 
> **QA Testing:**
> - *Include comprehensive steps on how to test the implementation (e.g., environment configuration, credentials, unit & integration testing)*
> - *(If possible) Include a GIF image and/or short 10-30 second video showing testing the implementation*
> 
> **Next Steps:** *(list the required next steps and/or considerations for future implementations)*

**Please consider the following points when posting a PR:**
- Reviewers should read each PR description thoroughly in order to test and understand the requested implementation.
- Each PR requires at least **2 approvals** in order to be merged into the main branch.
- Assign corresponding group members as reviewers to the PR (automatically notifies them for a faster review process)
- Assign the necessary labels (e.g., high/medium/low priority, bug, new feature, etc.)
- Each development task in Notion should correspond to **one** PR *(a PR should not include more than one task)*<br><br>

### Coding Conventions
Below are guidelines for keeping consistent code formatting across the project: 
- **Variable Naming**
  - Avoid abbreviations, acronyms, or shortened wordings (e.g., `submit_button` instead of `sbmt_btn`)
  - Use snake casing (e.g., `sum_all_values(..)`)<br><br>
- **Comments**
  - Avoid comments where necessary
    - *Comments quickly become outdated and require constant maintenance since they aren’t checked by the compiler, adding extra work. Relying on them also encourages poor coding practices instead of writing clear, modular, and self-explanatory code.*
    - Prioritize only adding comments explaining the 'why'. Why this code is needed and/or deviates from traditional implementations
  - Utilize `TODO` comments to plan and indicate future implementations (useful for not forgetting implementations and/or considerations)<br><br>


### Resource Materials
*TBD*<br><br>

### Authors
**Amina Al-Helali** - Lead Project Manager, Developer, UX Designer

**Aaron Chase** - Lead Developer, Development Manager

**Emma Souannhaphanh** - Sound Designer, Developer, UX Designer

**Vaniya Sharma** - UX Designer, Co-Project Manager, Developer

**Racha Ibrahim** - Co-Lead 3D Artist, UX Designer, Graphic Designer

**Amy Van** - Co-Lead 3D Artist

