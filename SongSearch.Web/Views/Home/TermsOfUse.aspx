<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<div class="cw-legal">
	<h2><%: Model.PageTitle %></h2>
		<p>
		The following terms and conditions of use govern your use of the www.WorldSongNet.com USA Web (“Site”.) The Site is made available by (OUR BUSINESS ENTINY) herein referred to as the “Site Proprietors”, “us”, “our”, “we” & “ourselves”.) We may change the Terms and Conditions from time to time, and at any time without notice to you, by posting such changes on the Site. BY USING THE SITE, YOU ACCEPT AND AGREE TO THESE TERMS AND CONDITIONS AS APPLIED TO YOUR USE OF THE SITE. If you do not agree to these Terms and Conditions, you may not access or otherwise use the Site. By using the Site following any modifications to the Terms and Conditions, you agree to be bound by any such modifications to the Terms and Conditions. 
		</p>
		<h3>Age Requirement for Use of the Site</h3>
			<p>
			This Site is not intended for use by persons under the age of 18 years old. You must be 18 years old or older to access or otherwise use the Site and related services. 
			</p>
		<h3>Services Available on the Site</h3>
			<p>
			We may make certain content and services available to visitors and registrants of the Site. For example, you may be able to read news, articles, interviews and other content, or search, browse, preview, view song information, download music, request quotes for music licenses, contact catalog representatives/owners, obtain quotes or licenses to use certain music for certain discrete purposes, monitor your account with us, invite and give other users administrative access to your music, post your company logo and/or personal contact information for other users to view, and browse through an online catalog of items available for purchase through our affiliates (these services are collectively referred to herein with the Site as the "Site").  
			</p>
		<h3>Price Quotes for Music Licenses</h3>
			<p>Through the Site, you may be able to request price quotes for licenses to use certain music for certain discrete purposes. To obtain such a quote, the Site will make available on the “Contact Us” page certain contact information that pertains to the compositions and/or master recordings listed as a search result.   You may contact the party listed on the “Contact Us” page for a quote using the contact information listed. The contact entity or person(s) listed may return to you a quote for the applicable license(s). A PRICE QUOTE IS NOT TO BE CONSIDERED AS AN OFFER OR THE ACCEPTANCE OF AN OFFER. IN NO WAY SHALL A PRICE QUOTE CREATE ANY AGREEMENT, LICENSE OR GRANT OF RIGHTS, AND EXCEPT AS EXPRESSLY SET FORTH ON THE SITE, ALL LICENSES WILL BE SUBJECT TO TERMS AND CONDITIONS WHICH ARE MUTUALLY AGREED TO AND EXECUTED OFF-LINE. 
			</p>
		<h3>Music Licenses</h3>
			<p>Through the Site, you may be able to obtain licenses to use certain music for certain discrete purposes by accepting the applicable license agreements (the "License Agreements") supplied to you by the contact entity or person(s) listed on the “Contact Us” page of the Site.  You agree to be bound by all of the terms and conditions of the relevant License Agreements entered into with the entity or person(s) listed on the “Contact Us” page of the Site and you hereby indemnify us and save and hold us and our successors and assigns, harmless from any and all liability, claims, demands, loss and damage (including reasonable counsel fees and court cost) arising out of any quote or license that you enter into on your behalf or a third parties behalf with the entity or person(s) listed on the “Contact Us” page of the Site.   
			</p>
		<h3>Proprietary Rights</h3>
			<p>All original content (excluding music owned or controlled by third parties) that is contained on the Site (including the results of any song data search conducted on the Site) is the property of us and is protected by all applicable Federal, State laws, as well as the applicable laws of jurisdictions outside the United States. Your use of the Site does not grant to you ownership of any content, features or materials you may access on the Site. Any commercial use of the Site is strictly prohibited, except as expressly allowed herein or otherwise expressly approved in writing by us. You may not copy, reproduce, distribute or otherwise commercially exploit any music or other content on the Site without our prior written consent. By invitation from us or the entity or person(s) listed on the “Contact Us” page of the Site, you may stream and/or download music for non-commercial use only. We reserve the right, at our sole discretion, to terminate any registration and deny access to the Site to any one, for any reason, without notice. Further, any such misuse of music, materials or content contained on the Site may violate copyright and other laws of the United States, other countries, as well as applicable state laws and may be subject to liability for such unauthorized use. We do not grant any license or other authorization to any user of our trademarks, service marks, other copyrightable material or any other intellectual property by including them on the Site. You also agree and acknowledge that any ideas, concepts, methods, systems, designs, plans, techniques or other similar materials that you submit or otherwise communicate to us or the Site may be used by us in any manner.
			</p>
		<h3>Non-exclusive Rights</h3>
			<p>Our partner JS Publishing, LLC (“JSP”) will have the non-exclusive right but not the obligation to market, represent and license the music that you include on the Site in all media now known or hereafter devised throughout the world.  Should JSP find a licensing or other exploitation placement opportunity for you or your music or a third party’s music that you have invited into the Site, JSP will contact you or the third party directly and negotiate in good faith what participation (if any) JSP will be due from such licensing and/or exploitation opportunity.  
			</p>
		<h3>User Information</h3>
			<p>
			In the course of your use of the Site, you may be asked to provide certain information to us (such information referred to hereinafter as "User Information"). Our information collection and use policies with respect to such User Information are set forth in the Site <%: Html.ActionLink("Privacy Policy", MVC.Home.PrivacyPolicy()) %> which is incorporated herein by reference for all purposes. You acknowledge and agree that you are solely responsible for the accuracy and content of the User Information. 
			</p>
		<h3>Unsolicited Materials</h3>
			<p>
			Unless specifically requested, we do not solicit nor do we wish to receive any confidential, secret or proprietary information or other material from you through the Site or any of its services by e-mail, or in any other way. Unless otherwise expressly agreed prior to your submission of information to us, any information or material submitted or sent to us will be deemed not to be confidential or secret. By submitting or sending information or other material to us you represent and warrant that the information is original to you and that no other party has any rights to the material. By submitting or sending information or other material (excluding music) to us you grant us the royalty-free, unrestricted, worldwide, perpetual, irrevocable, non-exclusive and fully sub-licensable right and license to use, reproduce, modify, adapt, publish, translate, create derivative works from, distribute, perform and display such material (in whole or part) worldwide and/or to incorporate it in other works in any form, media, or technology now known or later developed. You also warrant that any "moral rights" in posted materials have been waived. 
			</p>
		<h3>User Conduct</h3>
			<p>
			You warrant and agree that, while using the Site, you shall not upload, post or transmit to or distribute or otherwise publish through the Site any materials that:  
			</p>
			<ol>
				<li>are protected by copyright, or other proprietary or intellectual property right, or derivative works with respect thereto, except as provided herein or without first obtaining permission from us or the copyright owner;  
				</li><li>are unlawful, threatening, harassing, profane, tortuous, defamatory, vulgar, obscene, libelous, deceptive, fraudulent, contain explicit or graphic descriptions or accounts of sexual acts (including but not limited to sexual language of a violent or threatening nature directed at another individual or group of individuals), invasive of another's privacy, or hateful, 
				</li><li>restrict or inhibit any other user from using and enjoying the Site, 
				</li><li>constitute or encourage conduct that would constitute a criminal offense or give rise to civil liability, or 
				</li><li>contain a virus or other harmful component, advertising of any kind, or false or misleading indications of origin or statements of fact.  
			 </ol>
			<p>
			You also warrant and agree that you shall not:
			</p>
			 <ol>
				<li>impersonate or misrepresent your affiliation with any other person or entity;</li>  
				<li>upload, post, publish, transmit, reproduce, distribute or in any way exploit any information or other material obtained through the Site for commercial purposes (other than as expressly permitted by the provider of such information or other material);</li>  
				<li>engage in spamming or flooding;</li>   
				<li>reproduce or copy any audio files that are provided to you on the Site for any type of commercial use or commercial exploitation, or</li>    
				<li>attempt to gain unauthorized access to other computer systems through the Site. Except as otherwise expressly permitted herein, you may not upload, post, publish, reproduce, transmit or distribute in any way any component of the Site itself or derivative works with respect thereto, as the Site is copyrighted as a collective work under U.S. copyright laws.</li>  
			 </ol>
			 <p>
			We have no obligation to monitor any content on or through the Site and we assume no obligation. You acknowledge and agree, however, that we do retain the right to monitor the Site and to disclose any information as necessary or appropriate to satisfy any law, regulation or other governmental request, to operate the Site properly, or to protect ourselves or our users. We reserve the right to refuse to post or to remove any information or materials, in whole or in part, that, in our sole discretion, are unacceptable, undesirable, inappropriate or in violation of these Terms and Conditions. 
			</p>
			 <p>
			You agree to defend, indemnify and hold the Site Proprietors and their directors, officers, employees, agents and affiliates harmless from any and all claims, liabilities, costs and expenses, including reasonable attorneys' fees, arising in any way from your use of the Site or the placement or transmission of any message, content, information, software or other materials through the Site by you. 
			</p>
		<h3>Disclaimer Of Warranties</h3>
			<p>
			THE SITE, INCLUDING, WITHOUT LIMITATION, ALL CONTENT, FUNCTIONS AND MATERIALS, IS PROVIDED "AS IS," WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING, WITHOUT LIMITATION, ANY WARRANTY FOR INFORMATION, DATA, DATA PROCESSING SERVICES, OR UNINTERRUPTED ACCESS, ANY WARRANTIES CONCERNING THE AVAILABILITY, ACCURACY, USEFULNESS, OR CONTENT OF INFORMATION, AND ANY WARRANTIES OF TITLE, NON-INFRINGEMENT, MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. WE DO NOT WARRANT THAT THE SITE OR THE FUNCTIONS, FEATURES OR CONTENT CONTAINED THEREIN WILL BE TIMELY, SECURE, UNINTERRUPTED OR ERROR FREE, OR THAT DEFECTS WILL BE CORRECTED. WE MAKE NO WARRANTY THAT THE SITE WILL MEET USERS' REQUIREMENTS. NO ADVICE, RESULTS OR INFORMATION, WHETHER ORAL OR WRITTEN, OBTAINED BY YOU FROM US OR THROUGH THE SITE SHALL CREATE ANY WARRANTY NOT EXPRESSLY MADE HEREIN. IF YOU ARE DISSATISFIED WITH THE SITE, YOUR SOLE REMEDY IS TO DISCONTINUE USING THE SITE. 
			</p>
		<h3>Limitation Of Liability</h3>
			<p>IN NO EVENT SHALL THE SITE PROPRIETORS OR ANY OF THEIR DIRECTORS, OFFICERS, EMPLOYEES, AGENTS, AFFILIATES, OR CONTENT OR SERVICE PROVIDERS BE LIABLE FOR ANY INDIRECT, SPECIAL, INCIDENTAL, CONSEQUENTIAL, EXEMPLARY OR PUNITIVE DAMAGES ARISING FROM OR DIRECTLY OR INDIRECTLY RELATED TO THE USE OF, OR THE INABILITY TO USE, THE SITE OR THE CONTENT, MATERIALS AND FUNCTIONS RELATED THERETO, INCLUDING, WITHOUT LIMITATION, LOSS OF REVENUE, OR ANTICIPATED PROFITS OR LOST BUSINESS OR LOST SALES, EVEN IF SUCH ENTITY HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. SOME JURISDICTIONS DO NOT ALLOW THE LIMITATION OR EXCLUSION OF LIABILITY FOR INCIDENTAL OR CONSEQUENTIAL DAMAGES SO SOME OF THE ABOVE LIMITATIONS MAY NOT APPLY TO CERTAIN USERS. IN NO EVENT SHALL ANY SITE PROPRIETOR BE LIABLE FOR OR IN CONNECTION WITH ANY CONTENT POSTED, TRANSMITTED, EXCHANGED OR RECEIVED BY OR ON BEHALF OF ANY USER OR OTHER PERSON ON OR THROUGH THE SITE. IN NO EVENT SHALL THE TOTAL LIABILITY OF THE SITE PROPRIETORS TO YOU FOR ALL DAMAGES, LOSSES, AND CAUSES OF ACTION (WHETHER IN CONTRACT OR TORT, INCLUDING, BUT NOT LIMITED TO, NEGLIGENCE OR OTHERWISE) ARISING FROM THE TERMS AND CONDITIONS OR YOUR USE OF THE SITE EXCEED, IN THE AGGREGATE, $10.00. 
			</p>
		<h3>Links from and to the Site</h3>
			<p>You acknowledge and agree that we have no responsibility for the accuracy or availability of information provided by Web sites to which you may link from the Site ("Linked Sites"). Links to Linked Sites do not constitute an endorsement by or association with us of such sites or the content, products, advertising or other materials presented on such sites. We do not author, edit, or monitor these Linked Sites. You acknowledge and agree that we are not responsible or liable, directly or indirectly, for any damage or loss caused or alleged to be caused by or in connection with use of or reliance on any such content, goods or services available on such Linked Sites. 
			</p>
		<h3>Applicable Laws</h3>
			<p>We control and operate this Site from our offices in the United States of America. We do not represent that materials on the Site are appropriate or available for use in other locations. Persons who choose to access this Site from other locations do so on their own initiative, and are responsible for compliance with local laws, if and to the extent local laws are applicable. 
			</p>
		<h3>Miscellaneous</h3>
			<p>The Terms and Conditions and the relationship between you and us shall be governed by the laws of the State of California, without regard to its conflict of law provisions. You agree that any cause of action that may arise under the Terms and Conditions shall be commenced and be heard in an appropriate court in California in the County of Los Angeles. You agree to submit to the personal and exclusive jurisdiction of the courts located within Los Angeles County in the State of California. Our failure to exercise or enforce any right or provision of the Terms and Conditions shall not constitute a waiver of such right or provision. If any provision of the Terms and Conditions is found by a court of competent jurisdiction to be invalid, the parties nevertheless agree that the court should endeavor to give effect to the parties' intentions as reflected in the provision, and the other provisions of the Terms and Conditions remain in full force and effect. You agree that regardless of any statute or law to the contrary, any claim or cause of action arising out of or related to use of the Services or the Terms and Conditions must be filed within one (1) year after such claim or cause of action arose or be forever barred. 
			</p>
		<h3>Modifications to the Site and the Services</h3>
			<p>
			We reserve the right, for any reason, in our sole discretion, to terminate, change, suspend or discontinue any aspect of the Site, including, but not limited to, content, features or hours of availability. We may also impose limits on certain features of the Site or restrict your access to part or all of the Site without notice or penalty. 
			</p>
		<h3>Copyright Act Agent</h3>
			<p>
			We respect the intellectual property rights of others, and require that the people who use the Site do the same. If you believe that your work has been copied in a way that constitutes copyright infringement, please forward the following information to the Copyright Agent named below: 
			</p>
			<ul>
			<li>Your address, telephone number, and email address;</li>
			<li>A description of the copyrighted work that you claim has been infringed;</li>
			<li>A description of where the alleged infringing material is located;</li>
			<li>A statement by you that you have a good faith belief that the disputed use is not authorized by you, the copyright owner, its agent, or the law;
			</li><li>An electronic or physical signature of the person authorized to act on behalf of the owner of the copyright interest;
			</li><li>A statement by you, made under penalty of perjury, that the above information in your Notice is accurate and that you are the copyright owner or authorized to act on the copyright owner's behalf.
			</ul>
	</div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
